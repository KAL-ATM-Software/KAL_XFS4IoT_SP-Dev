/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "framework.h"
#include "tokens.h"
#include "endtoendsecurity.h"
#include <climits>

// We currently only support singgle currencies - hence one dispense token. 
char const DISPENSE_KEY_NAME[] = "DISPENSE";

// The details we extract from the dispense key
static struct DispenseKeyValues_t DispenseKeyValues = { 0, 0, "   " };
const struct DispenseKeyValues_t* const GetDispenseKeyValues()
{
    return &DispenseKeyValues;
}

static struct DispenseKeyValues_t RemainingValue = { 0, 0, "   " };

// Forward reference
bool ExtractValue(char const* const Start, char const* const End, unsigned long* valueOut);

extern bool CurrentTokenSet;

/// <summary>
/// Extract required details from a dispense Token. 
/// </summary>
/// <remarks>
/// The token _must_ be validated with ValidateToken before calling this. If 
/// validation fails, do not call this method. 
/// </remarks>
/// <param name="Token"></param>
/// <param name="TokenSize"></param>
/// <returns>if False, do not dispense</returns>
bool ParseDispenseToken(char const* const Token, size_t TokenSize)
{

    if (Token == NULL)
    {
        Log("ParseDispenseToken: Invalid token");
        CleanDispenceValues();
        return false;
    }

    LogV("ParseDispenseToken( Token=\"%.1024s\", TokenSize=%d )", Token, TokenSize);

    CleanDispenceValues();

    char const *const DispenseKeyStart = strstr(Token, DISPENSE_KEY_NAME);
    if (DispenseKeyStart == NULL)
    {
        Log("ParseDispenseToken: Failed to find dispense key");
        CleanDispenceValues();
        return false;
    }
    if( DispenseKeyStart[sizeof(DISPENSE_KEY_NAME)-1] != '1' || DispenseKeyStart[sizeof(DISPENSE_KEY_NAME)] != '=' )
    {
        Log("ParseDispenseToken: Invalid dispense key found. Only DISPENSE1 is currently supported. ");
        CleanDispenceValues();
        return false;
    }

    char const *const DispenseValueStart = DispenseKeyStart + sizeof(DISPENSE_KEY_NAME) +1;
    char const *const DispenseValueEnd = strstr(DispenseValueStart, ",");
    if (DispenseValueEnd == NULL)
    {
        Log("ParseDispenseToken: Invalid dispense key value");
        CleanDispenceValues();
        return false;
    }
    char const* const DuplicateDispenseKeyStart = strstr(DispenseKeyStart + sizeof(DISPENSE_KEY_NAME), DISPENSE_KEY_NAME) ;
    if (DuplicateDispenseKeyStart != NULL)
    {
        Log("ParseDispenseToken: Multiple dispense keys found. Only DISPENSE1 is currently supported. ");
        CleanDispenceValues();
        return false;
    }

    // At this point we're confident that between DispenseValueStart and DispenseValueEnd we have a 
    // string that should be something like 100.00EUR
    if( (DispenseValueEnd-DispenseValueStart) < 4 )
    {
        Log("ParseDispenseToken: Invalid dispense value string - too short.");
        CleanDispenceValues();
        return false;
    }

    // Currency must always be three characters long. 
    char const* const CurrencyStart = DispenseValueEnd - 3;
    if( !isalpha(CurrencyStart[0]) ||
        !isalnum(CurrencyStart[1]) ||
        !isalnum(CurrencyStart[2]) )
    {
        Log("ParseDispenseToken: Invalid currency");
        CleanDispenceValues();
        return false;
    }

    // The integer value starts from DispenseValueStart and either ends with a '.' 
    // or the first character of the currency code. 
    char const *ValueEnd = CurrencyStart;
    for (char const* c = DispenseValueStart; c < CurrencyStart; c++)
    {
        if (*c == '.')
        {
            ValueEnd = c;
            break;
        }
    }
    if(ValueEnd == DispenseValueStart)
    {
        Log("ParseDispenseToken: Missing integer value. Dispense value must not start with '.'");
        CleanDispenceValues();
        return false;
    }
    unsigned long value = 0;
    if (!ExtractValue(DispenseValueStart, ValueEnd, &value))
        return false; 

    // Deal with fraction
    unsigned long fraction = 0;
    if (ValueEnd != CurrencyStart)
    {
        char const* const FractionStart = ValueEnd + 1; 
        if(FractionStart == CurrencyStart)
        {
            Log("ParseDispenseToken: Invalid dispense value. No fractional value after '.' ");
            CleanDispenceValues();
            return false;
        }
        if (!ExtractValue(FractionStart, CurrencyStart, &fraction))
            return false;

    }

    // Commit results
    DispenseKeyValues.Value = value;
    DispenseKeyValues.Fraction = fraction;
    DispenseKeyValues.Currency[0] = CurrencyStart[0];
    DispenseKeyValues.Currency[1] = CurrencyStart[1];
    DispenseKeyValues.Currency[2] = CurrencyStart[2];

    memcpy_s(&RemainingValue, sizeof(RemainingValue), &DispenseKeyValues, sizeof(DispenseKeyValues));

    Log("ParseDispenseToken() ==> true");
    return true;

}

bool ExtractValue(char const* const Start, char const* const End, unsigned long *valueOut )
{
    // Decode integer part, from DispenseValueStart to ValueEnd.
    // We'll stick to 32 bit values - unlikely anyone will want to dispense 
    // more than 4294967295 units of any currency! That's ten digits.
    if ((End - Start) > 10)
    {
        Log("ParseDispenseToken: Dispense value part is too large. Maximum dispensable token is 4294967295");
        CleanDispenceValues();
        return false;
    }
    // Copy to a null terminated string so we can use strtoul.
    char ValueString[11];
    strncpy_s(ValueString, sizeof(ValueString), Start, End - Start);
    char* stopChar = NULL;
    errno = 0;
    unsigned long value = strtoul(ValueString, &stopChar, 10);
    if (value == ULONG_MAX && errno == ERANGE)
    {
        Log("ParseDispenseToken: Dispense value part is too large. Maximum dispensable token is 4294967295");
        CleanDispenceValues();
        return false;
    }
    *valueOut = value;

    return true; 
}

/// <summary>
/// Authorise the given amount based on the current token. 
/// If the amount is authorised then it will be subtracted from the remaining value for this token - 
/// this part of the value can't be dispensed again even if the dispense then fails. 
/// </summary>
/// <param name="Value">Unit part of the value</param>
/// <param name="Fraction">fractional part of the value</param>
/// <param name="Currency">Three char currency ID</param>
/// <returns></returns>
bool AuthoriseDispense(unsigned int Value, unsigned int Fraction, char const Currency[3] )
{
    LogV("AuthoriseAgainstToken( Value=%d, Fraction=%d, Currency=\"%c%c%c\" )", Value, Fraction, Currency[0], Currency[1], Currency[2]);

    // Sanity check that the requested currency matches. 
    // (Note - we only support a single currency per-token at the moment. )
    if (
        Currency[0] != RemainingValue.Currency[0] ||
        Currency[1] != RemainingValue.Currency[1] ||
        Currency[2] != RemainingValue.Currency[2]
        )
    {
        Log("AuthoriseAgainstToken: Requested dispense currency doesn't match the token currency");
        return false;
    }

    // Check that there's enough remaining on the current token
    if( Value > RemainingValue.Value || 
        Value == RemainingValue.Value && Fraction > RemainingValue.Fraction )
    {
        LogV("AuthoriseAgainstToken: Request to dispense more than the current token authorises. Remaining = %d.%d%c%c%c"
            , RemainingValue.Value, RemainingValue.Fraction, RemainingValue.Currency[0], RemainingValue.Currency[1], RemainingValue.Currency[2]);
        return false;
    }

    // Reduce the current token by the requested amount
    RemainingValue.Value -= Value; 
    RemainingValue.Fraction -= Fraction; 

    // If we've dispensed all of the value for this token then we can invalidate the token 
    // and allow a new token to be created and used. 
    if (RemainingValue.Value == 0 && RemainingValue.Fraction == 0)
    {
        LogV("AuthoriseAgainstToken: Full token value has been dispensed. The token/nonce will now be cleared");
        bool result = InvalidateToken();
        if (!result)
        {
            LogV("AuthoriseAgainstToken: Internal error - Token has been used up, but it couldn't be invalidated");
            return false;
        }
    }
    else
    {
        LogV("AuthoriseAgainstToken: Authorised=%d.%d%c%c%c, remaining=%d.%d%c%c%c",
             Value, Fraction, Currency[0], Currency[1], Currency[2],
             RemainingValue.Value, RemainingValue.Fraction, RemainingValue.Currency[0], RemainingValue.Currency[1], RemainingValue.Currency[2]
            );
    }

    LogV("AuthoriseAgainstToken: => true");
    return true;
}

/// <summary>
/// Do a complete authorisation against the token, taking into account if it's been seen before
/// and tracking the amount of cash already authorised against it. 
/// </summary>
/// <param name="Token">Full token string, including HMAC</param>
/// <param name="TokenLength">Length of the token string, including null</param>
/// <param name="Value">Unit value of dispense to authorise</param>
/// <param name="Fraction">Fractional value of dispense, to authorise</param>
/// <param name="Currency">Currency code of dispense to authorise</param>
/// <returns>false if the dispense should NOT happen. True if the dispense is authorised</returns>
bool AuthoriseDispenseAgainstToken(char const* Token, size_t TokenLength, unsigned int Value, unsigned int Fraction, char const Currency[3])
{
    LogV("AuthoriseDispenseAgainstToken( Token=\"%.1024s\", TokenSize=%d, Value=%d, Fraction=%d, Currency=\"%c%c%c\"  )", Token, TokenLength, Value, Fraction, Currency[0], Currency[1], Currency[2]);

    bool result = false;
    bool ExistingToken = CurrentTokenSet; 

    result = ValidateToken(Token, TokenLength);
    if (!result) 
    {
        Log("AuthoriseDispenseAgainstToken: => false");
        return false;
    }

    if (!ExistingToken)
    {
        result = ParseDispenseToken(Token, TokenLength);
        if (!result)
        {
            Log("AuthoriseDispenseAgainstToken: => false");
            return false;
        }
    }

    result = AuthoriseDispense(Value, Fraction, Currency);
    if (!result)
    {
        Log("AuthoriseDispenseAgainstToken: => false");
        return false;
    }

    LogV("AuthoriseDispenseAgainstToken: => true");
    return true;
}


void CleanDispenceValues()
{
    DispenseKeyValues.Value = 0;
    DispenseKeyValues.Fraction = 0;
    DispenseKeyValues.Currency[0] = DispenseKeyValues.Currency[1] = DispenseKeyValues.Currency[2] = ' ';

    RemainingValue.Value = 0;
    RemainingValue.Fraction = 0;
    RemainingValue.Currency[0] = DispenseKeyValues.Currency[1] = DispenseKeyValues.Currency[2] = ' ';
}
