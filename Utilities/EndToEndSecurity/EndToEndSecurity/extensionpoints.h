/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
#pragma once

// This file defines the functions that must be implemented to complete the end to end security 
// support. These will be implemented for each hardware device. 

#ifdef __cplusplus
#define C_LINKAGE "C"
#else 
#define C_LINKAGE
#endif 

/// <summary>
/// A programatic error has occured
/// </summary>
/// <remarks> 
/// Something unexpected and seriously wrong has happened. The system is likely to be unstable and 
/// may not continue to run. This could include errors up to and including memory corruption. 
/// </remarks> 
/// <param name="Message">A text message explaining what has happened</param>
extern C_LINKAGE void FatalError(char const* const Message);

/// <summary>
/// Log a text message for debugging.
/// </summary>
/// <param name="Message">Text message for debugging</param>
extern C_LINKAGE void Log(char const* const Message);

/// <summary>
/// Create a new Nonce value. 
/// </summary>
/// <remarks>
/// The firmware must create and track one nonce for use in command tokens. 
/// Note that there is no special handling needed for the responce token nonce - that's handled
/// by the host. 
/// The current command nonce does not need to be tracked across power-cycles. A power cycle should clear 
/// the current command nonce. However, the new nonce after the powercycle must be different to the previous 
/// values. 
/// NewNonce must return a pointer to a null terminated string containing a new nonce value. 
/// The nonce must be different to any proceeding nonce values, including across power-cycles. 
/// This could be done by persistently tracking an integer value and incrementing it. 
/// Alternatively it can be done by creating a long random number, using a hardware 
/// random number generator. 
/// The returned nonce pointer should remain valid for as long as the nonce is valid - 
/// i.e. until the nonce is cleared (or the machine is restarted.) 
/// </remarks>
/// <param name="Nonce">Output parameter pointing to the null terminated nonce string</param>
extern C_LINKAGE void NewNonce( char const ** Nonce );

/// <summary>
/// Compare given nonce string to the current stored nonce value. 
/// </summary>
/// <remarks>
/// Note that the comparison should be 'constant time' - if length of time taken to compare 
/// the values varies then this could be used by an attacker to find the correct value. An attacker 
/// can gradually vary the nonce to find the slowest comparison, which will match the correct nonce. 
/// </remarks>
/// <param name="CommandNonce">input nonce is _not_ null terminated</param>
/// <param name="NonceLength">Number of characters in the nonce</param>
/// <returns>return true if the value is correct</returns>
extern C_LINKAGE bool CompareNonce(char const* const CommandNonce, size_t NonceLength);

/// <summary>
/// Clear the current nonce value. 
/// </summary>
extern C_LINKAGE void ClearNonce();

/// <summary>
/// Create a new HMAC value for the given token. 
/// </summary>
/// <remarks> 
/// This routine must use the XFSAuthenticateDevice to generate the HMAC over the given string, 
/// and return that HMAC. 
/// Note that this should include all characters covered by TokenLength - this will include the 
/// "HMACSHA256=" text upto and including the '=' sign, as required by the XFS4IoT specification. 
/// </remarks> 
/// <param name="Token">Token string - not null terminated</param>
/// <param name="TokenLength">Number of characters to be used from the Token string</param>
/// <param name="TokenHMAC">A 32 byte binary buffer</param>
/// <returns></returns>
extern C_LINKAGE bool NewHMAC(char const *const Token, size_t  TokenLength, unsigned char *const TokenHMAC); 

/// <summary>
/// Check that the binary HMAC value matches the calculated value
/// </summary>
/// <remarks>
/// This is the core check on the token, to validate that it was created by a system that knows
/// the shared HMAC key. 
/// This routine must use the XFSAuthenticateHost key to calculate the HMAC over the given string,
/// and compare that to the given value. 
/// Note that the comparison should be 'constant time' - if length of time taken to compare 
/// the values varies then this could be used by an attacker to find the correct value. An attacker 
/// can gradually vary the nonce to find the slowest comparison, which will match the correct nonce. 
/// </remarks>
/// <param name="Token">Token string - not null terminated</param>
/// <param name="TokenLength">Number of characters to be used from the Token string</param>
/// <param name="TokenHMAC">A 32 byte binary buffer</param>
/// <returns>true if the HMAC matches the correct value for this token</returns>
extern C_LINKAGE bool CheckHMAC(char const *const Token, size_t TokenLength, unsigned char const* const TokenHMAC);

/// <summary>
/// Return the last dispense ID
/// </summary>
/// <remarks>
/// This value must be stored persistently - the last dispense ID should be valid across power-cycles. The 
/// dispense token handling will call SetLastDispenseID to store a new value. 
/// </remarks>
/// <param name="LastDispenseHMAC">32 byte buffer that should receive the last Dispense ID</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastDispenseID(bool* ValueKnown, char LastDispenseID[32]);

/// <summary>
/// Store the last dispense ID
/// </summary>
/// <remarks>
/// This value must be stored persistently - the last dispense ID should be valid across power-cycles. The 
/// dispense token handling will call SetLastDispenseID to store a new value. 
/// </remarks>
/// <param name="LastDispenseHMAC">32 byte buffer containing the value that should be stored</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool SetLastDispenseID(char LastDispenseID[32]);


/// <summary>
/// Return the last dispensed amount
/// </summary>
/// <remarks>
/// This value must be stored persistently - the last dispensed amount should be valid across power-cycles. 
/// </remarks>
/// <param name="UnitValue">unit part of value. e.g. $1</param>
/// <param name="SubUnitValue">fractional part. e.g. 50c </param>
/// <param name="Currency">Three character ISO currency code. e.g. "USD"</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastDispenseAmount(unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3]);

/// <summary>
/// Set Presented to true if the last dispensed bundle was accessable to the user
/// </summary>
/// <remarks>
/// This value must be stored persistently - the present status should be valid across power-cycles. 
/// </remarks>
/// <param name="Presented">Should be set to true if the last bundle was presented, otherwise false</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastDispensePresented(bool *Presented);

/// <summary>
/// If the value can be guaranteed, return the last amount that was retracted after being accessable to the customer.
/// </summary>
/// <remarks>
/// If notes are counted during a retract this will return the amount retracted. If the notes aren't counted, or the 
/// value of the notes retracted is not reliably known for any reason, ValueKnown should return false and the other 
/// values will be ignored. In this case, the RetractedAmount1 value will be "?".
/// </remarks>
/// <param name="ValueKnown">True if the retracted amount can be guaranteed</param> 
/// <param name="UnitValue">unit part of value. e.g. $1</param>
/// <param name="SubUnitValue">fractional part. e.g. 50c </param>
/// <param name="Currency">Three character ISO currency code. e.g. "USD"</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastRetractedAmount( bool *ValueKnown, unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3]);

/// <summary>
/// Return the last presented amount
/// </summary>
/// <remarks>
/// This value must be stored persistently - the last presented amount should be valid across power-cycles. 
/// </remarks>
/// <param name="UnitValue">unit part of value. e.g. $1</param>
/// <param name="SubUnitValue">fractional part. e.g. 50c </param>
/// <param name="Currency">Three character ISO currency code. e.g. "USD"</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastPresentedAmount(unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3]);

/// <summary>
/// Set Retracted to true if the last dispensed bundle was accessable to the user and retracted
/// </summary>
/// <remarks>
/// This value must be stored persistently - the present status should be valid across power-cycles. 
/// </remarks>
/// <param name="Retracted">Should be set to true if the last bundle was presented and retracted, otherwise false</param>
/// <returns>true if successful</returns>
extern C_LINKAGE bool GetLastDispenseRetracted(bool* Retracted);
