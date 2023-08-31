/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
#pragma once

// Token keys and other details

extern char const NonceStr[];
extern char const HMACSHA256Str[];
extern unsigned int HMACSHA256StrLen;
// length of the HMAC SHA256 string - 256 bit
#define HMACSHA256Len (64U)
extern char const TokenFormatStr[];
extern char const TokenLengthStr[];

// Absolute minimum token length, including required keys, including null. 
// NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2
extern unsigned int const MinTokenLength;

// Max permitted token length, as defined in XFS Spec. (In bytes, plus null)
extern unsigned int const MaxTokenLength;
