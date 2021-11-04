/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#pragma once

#ifdef __cplusplus
#define C_LINKAGE "C"
#else 
#define C_LINKAGE
#endif 

extern C_LINKAGE bool ValidateToken(char const *const Token, size_t TokenLength);
extern C_LINKAGE bool InvalidateToken();
extern C_LINKAGE bool ParseDispenseToken(char const *const Token, size_t TokenSize);
extern C_LINKAGE bool AuthoriseDispense(unsigned int UnitValue, unsigned int SubUnitValue, char const Currency[3]);

extern C_LINKAGE bool AuthoriseDispenseAgainstToken(char const *const Token, size_t TokenLength, unsigned int UnitValue, unsigned int SubUnitValue, char const Currency[3]);



void CleanDispenceValues();

struct DispenseKeyValues_t
{
    unsigned long Value;
    unsigned long Fraction;
    char Currency[3]; // NOT null terminated
};

extern C_LINKAGE const struct DispenseKeyValues_t *const GetDispenseKeyValues();
