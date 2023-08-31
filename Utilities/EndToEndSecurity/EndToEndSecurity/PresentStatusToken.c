/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "framework.h"
#include "tokens.h"
#include "extensionpoints.h"
#include "endtoendsecurity.h"

extern C_LINKAGE bool ValidateToken(char const* const Token, size_t TokenLength);
extern C_LINKAGE bool ParseDispenseToken(char const* const Token, size_t TokenSize);
extern C_LINKAGE bool AuthoriseDispense(unsigned int UnitValue, unsigned int SubUnitValue, char const Currency[3]);

// Arbitary limit on nonce length
unsigned int const MaxNonceLength = 100;

// Max permitted token length, as defined in XFS Spec. (In bytes, plus null)
#define MaxTokenLength ((unsigned int const)(1024 + 1))
char PresentTokenBuffer[MaxTokenLength] = "";

static char const NonceKey[] = "NONCE=";
static char const TokenFormatKey[] = "TOKENFORMAT=";
static char const TokenLengthKey[] = "TOKENLENGTH=";
static char const DispenseIDKey[] = "DISPENSEID=";
static char const Dispensed1Key[] = "DISPENSED1=";
static char const Presented1Key[] = "PRESENTED1=";
static char const YesString[] = "YES";
static char const NoString[] = "NO";
static char const PresentedAmount1Key[] = "PRESENTEDAMOUNT1=";
static char const Retracted1Key[] = "RETRACTED1=";
static char const RetractedAmount1Key[] = "RETRACTEDAMOUNT1=";
static char const HMACSHA256Key[] = "HMACSHA256=";


char* next = NULL;
char* end = NULL;

bool AddKey(char const* const name, char const* const format, ...)
{
    va_list vl;
    va_start(vl, format);
    int offset = vsprintf_s(next, end - next, format, vl);
    va_end(vl);

    if (offset == -1)
    {
        LogV("Buffer overflow adding %s to the present status token", name);
        return false;
    }
    next += offset;
    return true;
}


/// <summary>
/// Return the token used to protect the GetPresentStatus result data
/// </summary>
/// <param name="Nonce">A client supplied nonce string</param>
/// <param name="Token">The output paramter pointing to the token</param>
/// <returns>True if the token is valid. False on failures.</returns>
extern C_LINKAGE bool GetPresentStatusToken(char const* const Nonce, char const** Token)
{
    LogV("GetPresentStatusToken( Nonce=\"%.1024s\", Token=%d )", Nonce, Token);
    bool rc = false;

    // Parameter checking 
    if (Token == NULL)
    {
        Log("GetPresentToken: Invalid Token pointer.");
        return false;
    }
    *Token = NULL;

    if (Nonce == NULL || Nonce[0] == '\0')
    {
        Log("GetPresentToken: Invalid nonce - null or empty string.");
        return false;
    }

    if (strlen(Nonce) > MaxNonceLength)
    {
        Log("GetPresentToken: Nonce too long.");
        return false;
    }

    // Construct the token. 
    // Sample token from the XFS specification
    // NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0268,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=50.00EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.00EUR,RETRACTED1=YES,RETRACTEDAMOUNT1=?,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83
    //
    // NONCE            : Passed by client
    // TOKENFORMAT      : Fixed - 1
    // TOKENLENGTH      : Dynamically calculated. Always four digits
    // DISPENSEID       : Stored from last dispense command
    // DISPENSED1       : Recorded by hardware
    // PRESENTED1       : Recorded by hardware
    // PRESENTEDAMOUNT1 : Recorded by hardware
    // RETRACTED1       : Recorded by hardware
    // RETRACTEDAMOUNT1 : Recorded by hardware
    // HMACSHA256       : Dynamically calculated

    // Set values used by AddKey to track current point in the token buffer. 
    next = PresentTokenBuffer;
    end = PresentTokenBuffer + sizeof(PresentTokenBuffer);

    // Nonce
    if (!AddKey(NonceKey, "%s", NonceKey)) return false;
    if (!AddKey("nonce value", "%s", Nonce)) return false;

    // Token format
    if (!AddKey(TokenFormatKey, ",%s", TokenFormatKey)) return false;
    if (!AddKey("TokenFormat value", "%s", "1")) return false;

    // Token lengths - placeholder. 
    if (!AddKey(TokenLengthKey, ",%s", TokenLengthKey)) return false;
    char* const lengthOffset = next; // Remember the possition of the length value so we can write it later. 
    if (!AddKey("TokenLength value", "%4.4d", 0)) return false;

    // Dispense ID
    char LastDispenseHMAC[32];
    bool LastDispenseHMACKnown = false;
    rc = GetLastDispenseID(&LastDispenseHMACKnown, LastDispenseHMAC);
    if( rc != true )
    {
        LogV("Failed to read the last dispense ID");
        return false;
    }
    // By XFS spec, if last dispense ID is unknown, this field is not included. 
    if (LastDispenseHMACKnown == true)
    {
        if (!AddKey(DispenseIDKey, ",%s", DispenseIDKey)) return false;
        for (unsigned int i = 0; i < 32; i++)
        {
            if (!AddKey("DispenseID value", "%2.2hhX", LastDispenseHMAC[i])) return false;
        }
    }

    // Dispensed (amount)
    unsigned int UnitValue;
    unsigned int SubUnitValue;
    char Currency[3];
    rc = GetLastDispenseAmount(&UnitValue, &SubUnitValue, Currency);
    if (rc != true)
    {
        LogV("Failed to read the last dispensed amount");
        return false;
    }

    if (!AddKey(Dispensed1Key, ",%s", Dispensed1Key)) return false;
    if (!AddKey("Dispensed1 Value", "%d.%d%c%c%c", UnitValue, SubUnitValue, Currency[0], Currency[1], Currency[2])) return false;

    // Presented
    bool Presented = true; // assume the worst - customer tampered with the currency;
    rc = GetLastDispensePresented(&Presented);
    if (rc != true)
    {
        LogV("Failed to read the last dispense presented status");
        return false;
    }
    if (!AddKey(Presented1Key, ",%s", Presented1Key)) return false;
    if (!AddKey("Presented1 Value", "%s", Presented ? YesString : NoString)) return false;

    // PresentedAmount
    UnitValue = SubUnitValue = 0;
    Currency[0] = Currency[1] = Currency[2] = ' ';
    rc = GetLastPresentedAmount(&UnitValue, &SubUnitValue, Currency);
    if (rc != true)
    {
        LogV("Failed to read the last presented amount");
        return false;
    }

    if (!AddKey(PresentedAmount1Key, ",%s", PresentedAmount1Key)) return false;
    if (!AddKey("PresentedAmount1 Value", "%d.%d%c%c%c", UnitValue, SubUnitValue, Currency[0], Currency[1], Currency[2])) return false;

    // Retracted
    bool Retracted = true;
    rc = GetLastDispenseRetracted(&Retracted);
    if (rc != true)
    {
        LogV("Failed to read the last dispense retracted status");
        return false;
    }

    if (!AddKey(Retracted1Key, ",%s", Retracted1Key)) return false;
    if (!AddKey("Retracted1 Value", "%s", Retracted ? YesString : NoString)) return false;

    // RetractedAmount 
    bool RetractedValueKnown = false; 
    UnitValue = SubUnitValue = 0;
    Currency[0] = Currency[1] = Currency[2] = ' ';
    rc = GetLastRetractedAmount( &RetractedValueKnown, &UnitValue, &SubUnitValue, Currency);
    if (rc != true)
    {
        LogV("Failed to read the last retracted amount");
        return false;
    }

    if (!AddKey(RetractedAmount1Key, ",%s", RetractedAmount1Key)) return false;
    if (RetractedValueKnown)
    {
        if (!AddKey("RetractedAmount1 Value", "%d.%d%c%c%c", UnitValue, SubUnitValue, Currency[0], Currency[1], Currency[2])) return false;
    }
    else
    {
        if (!AddKey("RetractedAmount1 Value", "?")) return false;
    }

    // HMACSHA256 - Find HMAC
    static unsigned char TokenHMAC[32];
    if (!NewHMAC(PresentTokenBuffer, (size_t)(end - PresentTokenBuffer), TokenHMAC))
    {
        Log("GetPresentStatusToken: Failed to create HMAC value => false");
        return false;
    }

    if (!AddKey(HMACSHA256Key, ",%s", HMACSHA256Key)) return false;
    // HMACSHA256 - Convert token HMAC binary to HEX
    static const char HEXDigits[16] = "0123456789ABCDEF";
    for (unsigned int i = 0; i < 32; i++)
    {
        if (!AddKey("HMACSHA256 value", "%2.2hhX", TokenHMAC[i])) return false;
    }

    // Token Length - Write the string length into the token
    char lengthBuffer[5]; 
    sprintf_s(lengthBuffer, sizeof(lengthBuffer), "%4.4zd", strlen(PresentTokenBuffer));
    memcpy_s(lengthOffset, end - lengthOffset, lengthBuffer, sizeof(lengthBuffer)-1);


    *Token = PresentTokenBuffer;
    // All done. All good. 
    LogV("GetPresentStatusToken: => true");
    return true;
}