/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "framework.h"
#include "tokens.h"
#include "extensionpoints.h"

// Utility functions
bool CheckNibble(char nibble);
unsigned int ConvertHex(char high, char low);

// Create a reference to the Nonce handling functions - this is just to create a compile error 
// if these functions aren't implemented. 
void* Pull1 = NewNonce; 
void* Pull2 = ClearNonce; 

char const NonceStr[] = "NONCE";
char const HMACSHA256Str[] = "HMACSHA256";
// length of the HMAC SHA256 string - 256 bit
#define HMACSHA256Len (64U)
char const TokenFormatStr[] = "TOKENFORMAT";
char const TokenLengthStr[] = "TOKENLENGTH";

// Absolute minimum token length, including required keys, including null. 
// NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2
unsigned int const MinTokenLength = sizeof(NonceStr) - 1 + 2 + 1 +          // NONCE=1,
                                    sizeof(TokenFormatStr) - 1 + 2 + 1 +    // TOKENFORMAT=1,
                                    sizeof(TokenLengthStr) - 1 + 2 + 4 +    // TOKENLENGTH=0164,
                                    sizeof(HMACSHA256Str) - 1 + 1 + HMACSHA256Len + //HMACSHA256=1234567890123456789012345678901234567890123456789012345678901234
                                    1;                                    // null terminated

// Max permitted token length, as defined in XFS Spec. (In bytes, plus null)
unsigned int const MaxTokenLength = 1024 + 1;



/// <summary>
/// Validate that a token has a valid format.
/// </summary>
/// <description> 
/// Takes a token string and reports if it is valid or not. If the token is valid then it is safe 
/// to proceed with the operation protected by the token. If not, the operation should fail with an 
/// error. 
/// The token still is assumed to be unsafe and may have been passed by an attacker. Everything 
/// possible will be done to avoid invalid behaviour due to a hostile token. 
/// </description>
/// 
/// <param name="Token">Null terminated token string</param>
/// <param name="TokenSize">Token buffer size, including null</param>
/// <returns>true or false</returns>
bool ValidateToken(char const* const Token, size_t TokenSize)
{
    LogV("ValidateToken( Token=\"%.1024s\", TokenSize=%d )", Token, TokenSize);

    // Parameter checking. 
    // Consider the token to be an untrusted value, so maximum validity checking. 
    // Null token
    if (Token == NULL)
    {
        Log("ValidateToken: Null token => false");
        return false;
    }

    size_t TokenStringLength = strlen(Token) + 1;       // Plus null
    // Zero length string.
    if (TokenStringLength == 1)
    {
        Log("ValidateToken: Empty token => false");
        return false;
    }
    // Buffer length and string size don't match
    if (TokenStringLength != TokenSize)
    {
        Log("ValidateToken: TokenSize didn't match token length => false");
        return false;
    }
    // Token is too short to be valid
    if (TokenStringLength < MinTokenLength)
    {
        Log("ValidateToken: Token is too short => false");
        return false;
    }
    // Token is too long (and might cause buffer overflows.) 
    if (TokenStringLength > MaxTokenLength)
    {
        Log("ValidateToken: Token is too long. Max length=1024 bytes => false");
        return false;
    }

    // Parse
    // Check format
    bool inKeyName = true; // true for key name, false for value
    int symbolLength = 0; 
    for (char const* offset = Token; offset < Token + TokenStringLength - 1; offset++)
    {
        char thisChar = *offset;
        if (inKeyName)
        {
            if (thisChar == '=')
            { 
                if (symbolLength == 0)
                {
                    Log("ValidateToken: Missing key name");
                    return false; 
                }
                inKeyName = false; 
                symbolLength = 0; 
            }
            else if (!isalnum(thisChar))
            {
                Log("ValidateToken: Invalid character in key name");
                return false;
            }
            else
                symbolLength++;
        }
        else
        {
            if (thisChar == ',')
            {
                if (symbolLength == 0)
                {
                    Log("ValidateToken: Missing value");
                    return false;
                }
                inKeyName = true;
                symbolLength = 0; 
            }
            else if (!isalnum(thisChar) && thisChar!='.')
            {
                Log("ValidateToken: Invalid character in value");
                return false;
            }
            else
                symbolLength++;
        }
    }
    if (inKeyName || symbolLength == 0)
    {
        Log("ValidateToken: Missing value");
        return false; 
    }

    // Find Nonce
    char const *const nonceStrOffset = strstr(Token, NonceStr);
    if (nonceStrOffset != Token)
    {
        Log("ValidateToken: First key must be NONCE => false");
        return false;
    }
    char const* const nonceValOffset = strchr(Token, '=') + 1; // Skip over '='
    if (nonceValOffset == (char*)1)
    {
        Log("ValidateToken: NONCE doesn't have a value => false");
        return false;
    }
    char const* const nonceValEnd = strchr(Token, ',');
    if (nonceValEnd == 0)
    {
        Log("ValidateToken: NONCE value is invalid => false");
        return false;
    }

    // Find HMAC
    char const* const HMACStrOffset = strstr(Token, HMACSHA256Str);
    if (HMACStrOffset == 0)
    {
        Log("ValidateToken: No HMAC key found => false");
        return false;
    }
    // HMAC must be 64 characters
    size_t HMACLen = TokenStringLength - (HMACStrOffset - Token) - (sizeof(HMACSHA256Str) + 1);
    if ( HMACLen != 64 )
    {
        LogV("ValidateToken: HMACSHA256 value is too short. %d bytes, should be 64 => false", (HMACLen-/*HMACSHA256=*/11 -1));
        return false; 
    }
    static char TokenHMAC[32]; 
    char const* bytePtr = HMACStrOffset + sizeof(HMACSHA256Str);
    for (unsigned int i = 0; i < 32; i++)
    {
        char highNibble = *bytePtr++;
        char lowNibble  = *bytePtr++;
        if (!CheckNibble(highNibble) || !CheckNibble(lowNibble))
        {
            LogV("ValidateToken: Non-HEX byte in HMAC (%c%c) => false", highNibble, lowNibble );
            return false; 
        }
        TokenHMAC[i] = ConvertHex(highNibble, lowNibble);
    }

    if (TokenStringLength <= HMACSHA256Len) FatalError("Unexpected token length");
    unsigned int TokenExcludingHMACLen = (unsigned int)(TokenStringLength - HMACSHA256Len -1);
    if (!CheckHMAC(Token, TokenExcludingHMACLen,  TokenHMAC))
    {
        LogV("ValidateToken: Invalid HMAC => false");
        return false;
    }

    // Find other keys
    // 
    // Check Token Nonce matches current nonce
    if (CompareNonce(nonceValOffset, nonceValEnd - nonceValOffset) == false)
    {
        LogV("ValidateToken: Token nonce doesn't match current hardware nonce.");
        return false; 
    }

    // Check HMAC matches calculated HMAC


    LogV("ValidateToken: => true");
    return true;
}

bool CheckNibble(char nibble)
{
    return !(nibble < 'A' && nibble >'F' && nibble < '0' && nibble >'9');
}

unsigned int ConvertNibble(char nibble);
unsigned int ConvertHex(char high, char low)
{
    return ConvertNibble(high) << 4 | ConvertNibble(low);
}

unsigned int ConvertNibble(char nibble)
{
    if (nibble >= '0' && nibble <= '9') return nibble - '0';
    else if (nibble >= 'A' && nibble <= 'F') return nibble - 'A' + 10;

    // Should have been checked before calling this. 
    FatalError("Invalid nibble");
    return 0; 
}

/// <summary>
/// VArg version of Log()
/// </summary>
char LogVBuffer[2048];
void LogV(char const* const Message, ...)
{
    va_list vl;
    va_start(vl, Message);
    vsprintf_s(LogVBuffer, sizeof(LogVBuffer), Message, vl);
    va_end(vl);

    Log(LogVBuffer);
}

