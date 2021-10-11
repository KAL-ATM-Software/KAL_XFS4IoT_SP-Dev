/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
#pragma once

// Token keys and other details

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

