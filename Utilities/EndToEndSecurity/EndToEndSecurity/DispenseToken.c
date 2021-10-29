/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "framework.h"
#include "tokens.h"
#include "endtoendsecurity.h"

// We currently only support singgle currencies - hence one dispense token. 
char const DISPENSE_KEY_NAME[] = "DISPENSE";

// The details we extract from the dispense key
static struct DispenseKeyValues_t DispenseKeyValues = { 0, 0, "   " };
const struct DispenseKeyValues_t* const GetDispenseKeyValues()
{
    return &DispenseKeyValues;
}

// Forward reference
bool ExtractValue(char const* const Start, char const* const End, unsigned long* valueOut);

/// <summary>
/// Extract required details from a dispense Token. 
/// </summary>
/// <remarks>
/// The token _must_ be validated with ValidateToken before calling this. If 
/// validation fails, do not call this method. 
/// </remarks>
/// <param name="Token"></param>
/// <param name="TokenSize"></param>
/// <returns></returns>
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

void CleanDispenceValues()
{
    DispenseKeyValues.Value = 0;
    DispenseKeyValues.Fraction = 0;
    DispenseKeyValues.Currency[0] = DispenseKeyValues.Currency[1] = DispenseKeyValues.Currency[2] = ' ';
}
