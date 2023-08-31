/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "CppUnitTest.h"
#include "EndToEndSecurity.h"

using namespace std; 
using namespace Microsoft::VisualStudio::CppUnitTestFramework;

extern C_LINKAGE bool ValidateToken(char const* const Token, size_t TokenLength);
extern C_LINKAGE bool InvalidateToken();
extern C_LINKAGE bool ParseDispenseToken(char const* const Token, size_t TokenSize);
extern C_LINKAGE bool AuthoriseDispense(unsigned int UnitValue, unsigned int SubUnitValue, char const Currency[3]);


// The following extern "C" functions are the implementation of the firmware specific parts required 
// for end to end security. Here we have stub functions for use with the unit tests. 
extern "C" void Log(char const* const Message)
{
    Logger::WriteMessage( Message );
};

static std::string CurrentNonce = "";
static std::string ExpectedHMAC = "";
static std::string LastToken = ""; 

extern "C" void FatalError(char const* const Message)
{
    Assert::Fail(ToString(Message).c_str());
}

extern "C" void NewNonce(char const ** OutNonce)
{
    // Value will be set by tests - not the normal nonce behaviour! 
    *OutNonce = CurrentNonce.c_str(); 
}

extern "C" void ClearNonce()
{
    CurrentNonce = "";
    LastToken = "";
}

extern "C" bool CompareNonce(char const* const CommandNonce, unsigned long NonceLength )
{
    std::string nonce = std::string(CommandNonce, NonceLength);
    return nonce == CurrentNonce;
}

static bool NewHMACResult = false;
static std::string NewHMACString = "";
unsigned char ConvertHex(char high, char low);
extern "C" bool NewHMAC(char const* const Token, size_t TokenLength, unsigned char* const TokenHMAC)
{
    Assert::IsNotNull(Token);
    Assert::IsNotNull(TokenHMAC);

    if (!NewHMACResult) return false;

    static char TokenHMACString[65] = "";
    for (unsigned int i = 0; i < 32; i++)
    {
        TokenHMAC[i] = ConvertHex( NewHMACString[i*2], NewHMACString[i*2+1]);
    }

    NewHMACString = "";
    return true;
}

unsigned char ConvertNibble(char nibble);
unsigned char ConvertHex(char high, char low)
{
    return ConvertNibble(high) << 4 | ConvertNibble(low);
}

unsigned char ConvertNibble(char nibble)
{
    if (nibble >= '0' && nibble <= '9') return nibble - '0';
    else if (nibble >= 'A' && nibble <= 'F') return nibble - 'A' + 10;

    Assert::Fail(L"Invalid HMAC string given in a unit test");
    return 0;
}


extern "C" bool CheckHMAC(char const* const Token, size_t TokenLength, unsigned char const* const TokenHMAC)
{
    Assert::IsNotNull(Token);
    Assert::IsNotNull(TokenHMAC);

    static char TokenHMACString[65] ="";
    for (unsigned int i = 0; i < 32; i++)
    {
        sprintf_s((TokenHMACString+(i*2)),3, "%1X%1X", (TokenHMAC[i] & 0xF0) >> 4, TokenHMAC[i] & 0x0F);
    }

    bool result = std::string(TokenHMACString,64) == ExpectedHMAC; 

    ExpectedHMAC = ""; 
    LastToken = std::string(Token, TokenLength);

    return result;
}

static bool GetLastDispenseResult = false; 
static int LastDispensedUnit = -1;
static int LastDispensedSubUnit = -1; 
static std::string LastDispencedCurrency; 

#define HMACSHA256SIZE 32
static bool SetLastDispenseIDResult = false; 
static char LastDispenseID[HMACSHA256SIZE] = { 0 };
extern "C" bool SetLastDispenseID(char NewDispenseID[HMACSHA256SIZE])
{
    if (SetLastDispenseIDResult != true) return false; 

    Assert::IsNotNull(NewDispenseID);
    memcpy_s(LastDispenseID, sizeof(LastDispenseID), NewDispenseID, HMACSHA256SIZE);
    return true; 
}

static bool GetLastDispenseIDResult = false; 
static bool GetLastDispenseIDKnown = false;
extern "C" bool GetLastDispenseID(bool* ValueKnown, char DispenseID[HMACSHA256SIZE])
{
    if (GetLastDispenseIDResult != true) return false;

    Assert::IsNotNull(ValueKnown);
    *ValueKnown = GetLastDispenseIDKnown;

    Assert::IsNotNull(DispenseID);
    memcpy_s(DispenseID, HMACSHA256SIZE, LastDispenseID, sizeof(LastDispenseID) );
    return true;
}

extern "C" bool GetLastDispenseAmount(unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3])
{
    if (!GetLastDispenseResult) return false; 

    Assert::IsNotNull(UnitValue);
    Assert::IsNotNull(SubUnitValue);
    Assert::IsNotNull(Currency);

    *UnitValue = LastDispensedUnit; 
    *SubUnitValue = LastDispensedSubUnit; 
    Assert::AreEqual(size_t(3), LastDispencedCurrency.length());
    Currency[0] = LastDispencedCurrency[0];
    Currency[1] = LastDispencedCurrency[1];
    Currency[2] = LastDispencedCurrency[2];

    return true;
}

static bool GetLastDispensePresentedResult = false; 
static bool LastDispensePresented = true;

extern "C" bool GetLastDispensePresented(bool* Presented)
{
    if (!GetLastDispensePresentedResult) return false;

    Assert::IsNotNull(Presented);

    *Presented = LastDispensePresented; 

    return true;
}

static bool GetLastPresentedAmountResult = false;
static int LastPresentedAmountUnit = -1;
static int LastPresentedAmountSubUnit = -1;
static std::string LastPresentedAmountCurrency;

extern "C" bool GetLastPresentedAmount(unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3])
{
    if (!GetLastPresentedAmountResult) return false;

    Assert::IsNotNull(UnitValue);
    Assert::IsNotNull(SubUnitValue);
    Assert::IsNotNull(Currency);

    *UnitValue = LastPresentedAmountUnit;
    *SubUnitValue = LastPresentedAmountSubUnit;
    Assert::AreEqual(size_t(3), LastPresentedAmountCurrency.length());
    Currency[0] = LastPresentedAmountCurrency[0];
    Currency[1] = LastPresentedAmountCurrency[1];
    Currency[2] = LastPresentedAmountCurrency[2];

    return true;
}

static bool GetLastDispenseRetractedResult = false;
static bool LastDispenseRetracted = false;

extern "C" bool GetLastDispenseRetracted(bool* Retracted)
{
    if (!GetLastDispenseRetractedResult) return false;

    Assert::IsNotNull(Retracted);

    *Retracted = LastDispenseRetracted;

    return true;
}

static bool GetLastRetractedAmountResult = false;
static bool LastRetractedAmountKnown = false; 
static int LastRetractedAmountUnit = -1;
static int LastRetractedAmountSubUnit = -1;
static std::string LastRetractedAmountCurrency;

extern "C" bool GetLastRetractedAmount(bool *ValueKnown, unsigned int* UnitValue, unsigned int* SubUnitValue, char Currency[3])
{
    if (!GetLastRetractedAmountResult) return false;

    Assert::IsNotNull(ValueKnown);
    Assert::IsNotNull(UnitValue);
    Assert::IsNotNull(SubUnitValue);
    Assert::IsNotNull(Currency);

    *ValueKnown = LastRetractedAmountKnown;
    *UnitValue = LastRetractedAmountUnit;
    *SubUnitValue = LastRetractedAmountSubUnit;
    Assert::AreEqual(size_t(3), LastRetractedAmountCurrency.length());
    Currency[0] = LastRetractedAmountCurrency[0];
    Currency[1] = LastRetractedAmountCurrency[1];
    Currency[2] = LastRetractedAmountCurrency[2];

    return true;
}


namespace EndToEndSecurityTest
{
    /// <summary>
    /// Generic tests of token handling. 
    /// </summary>
    TEST_CLASS(GenericTokenTest)
    {
    public:

        TEST_METHOD(TokenMinLength)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));

            Assert::IsTrue(result);
        }
        TEST_METHOD(TokenTooShort)
        {
            // HMAC one character too short. 
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0163,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(TokenVeryLong)
        {
            CurrentNonce = "1";
            // Maximum token length = 1024 bytes
            //                        1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=1024,X=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" // 100
                "2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222" // 200
                "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" // 300
                "4444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444" // 400
                "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555" // 500
                "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" // 600
                "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777" // 700
                "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888" // 800
                "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" // 900
                "000000000000000000000000000000000000000000000000,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A734" //1000
               //123456789012345678901234"; // 1024
                "7B15434916FEA6AC16F3D2F2"; // 1024
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);
        }
        TEST_METHOD(TokenTooLong)
        {
            CurrentNonce = "1";
            // Maximum token length = 1024 bytes
            //                        0000000001111111111222222222233333333334444444444555555555566666666667777777777888888888899999999990
            //                        1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=1025,X=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" // 100
                "2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222" // 200
                "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" // 300
                "4444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444" // 400
                "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555" // 500
                "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" // 600
                "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777" // 700
                "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888" // 800
                "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" // 900
                "0000000000000000000000000000000000000000000000000,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A73" //1000
               //1234567890123456789012345"; // 1024
                "47B15434916FEA6AC16F3D2F2"; // 1025
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(TokenVeryTooLong)
        {
            CurrentNonce = "1";
            // Maximum token length = 1024 bytes
            //                        0000000001111111111222222222233333333334444444444555555555566666666667777777777888888888899999999990
            //                        1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=1025,X=AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" // 100
                "2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222" // 200
                "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" // 300
                "4444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444" // 400
                "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555" // 500
                "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" // 600
                "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777" // 700
                "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888" // 800
                "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" // 900
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" // 1000
                "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" // 1100
                "2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222" // 1200
                "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" // 1300
                "4444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444" // 1400
                "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555" // 1500
                "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" // 1600
                "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777" // 1700
                "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888" // 1800
                "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" // 1900
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000" // 2000
                "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111" // 2100
                "2222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222222" // 2200
                "3333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" // 2300
                "4444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444444" // 2400
                "5555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555555" // 2500
                "6666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666" // 2600
                "7777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777777" // 2700
                "8888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888" // 2800
                "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" // 2900
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"; // 2000
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(TokenEmptyString)
        {
            CurrentNonce = "1";
            char const testToken[] = "";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(TokenNull)
        {
            CurrentNonce = "1";
            char const* const testToken = nullptr;
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }

        TEST_METHOD(KeyNameMissing)
        {
            CurrentNonce = "1";
            char const testToken[] = "NONCE=1,=2,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(KeyMissing)
        {
            CurrentNonce = "1";
            char const testToken[] = "NONCE=1=2,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(ValueMissing)
        {
            CurrentNonce = "1";
            char const testToken[] = "NONCE=,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(KeyEqualsMissing)
        {
            CurrentNonce = "1";
            char const testToken[] = "NONCE=1,2,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(NoHMAC)
        {
            CurrentNonce = "1";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA000=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(MACTooSort)
        {
            CurrentNonce = "12";
            char const testToken[] = "NONCE=12,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(MACNoValue)
        {
            CurrentNonce = "12";
            char const testToken[] = "NONCE=12,TOKENFORMAT=1,TOKENLENGTH=0164,VALUE=11111111111111111111111111111111111111111111111111111111,HMACSHA256=";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(MACInvalidKey)
        {
            CurrentNonce = "12";
            char const testToken[] = "NONCE=12,TOKENFORMAT=1,TOKENLENGTH=0164,VALUE=111111111111111111111111111111111111111111111111111111111,HMACSHA256";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
    };

    /// <summary>
    /// Generic tests around command Nonce handling
    /// </summary>
    TEST_CLASS(NonceTest)
    {
    public:

        TEST_METHOD(LongNonce)
        {
            CurrentNonce = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);
        }
        TEST_METHOD(NoNonce)
        {
            CurrentNonce = "1";
            char const testToken[] = "MISNG=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(NonceNotMatching)
        {
            CurrentNonce = "12345";
            char const testToken[] = "NONCE=54321,TOKENFORMAT=1,TOKENLENGTH=0164,VALUE=111111111111111111111111111111111111111111111111111111111,HMACSHA256";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
        TEST_METHOD(NonceCleared)
        {
            CurrentNonce = "";
            char const testToken[] = "NONCE=54321,TOKENFORMAT=1,TOKENLENGTH=0164,VALUE=111111111111111111111111111111111111111111111111111111111,HMACSHA256";
            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsFalse(result);
        }
    };

    /// <summary>
    /// Generic tests around command HMAC checking
    /// </summary>
    TEST_CLASS(HMACTest)
    {
    public:

        TEST_METHOD(ValidHMAC)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));

            Assert::IsTrue(result);
            // ExpectedHMAC should have been reset when CheckHMAC was called. 
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string("NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256="), LastToken);
        }
        TEST_METHOD(WrongHMAC)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "11223344556677889900AABBCCDDEEFF11223344556677889900AABBCCDDEEFF";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));

            Assert::IsFalse(result);
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string("NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256="), LastToken);
        }
    };

    /// <summary>
    /// Tests specific to Dispense tokens
    /// </summary>
    TEST_CLASS(DispenserTest)
    {
    public: 
        TEST_METHOD(NullToken)
        {
            // Valid but unsupported. 
            auto result = ParseDispenseToken(NULL, 123);
            AssertCleanFail(result);
        }
        TEST_METHOD(ValidDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            auto values = GetDispenseKeyValues();

            Assert::IsTrue(result);
            Assert::AreEqual(unsigned long(123), values->Value);
            Assert::AreEqual(unsigned long(678), values->Fraction);
            Assert::AreEqual(std::string("XYZ"), std::string(values->Currency,3));
        }
        TEST_METHOD(MinimumValidDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=1XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            auto values = GetDispenseKeyValues();

            Assert::IsTrue(result);
            Assert::AreEqual(unsigned long(1), values->Value);
            Assert::AreEqual(unsigned long(0), values->Fraction);
            Assert::AreEqual(std::string("XYZ"), std::string(values->Currency,3));
        }
        TEST_METHOD(MaximumValidDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=4294967295XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            auto values = GetDispenseKeyValues();

            Assert::IsTrue(result);
            Assert::AreEqual(unsigned long(4294967295), values->Value);
            Assert::AreEqual(unsigned long(0), values->Fraction);
            Assert::AreEqual(std::string("XYZ"), std::string(values->Currency,3));
        }
        TEST_METHOD(GreaterThanMaximumValidDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=4294967296XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(LongerThanMaximumValidDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=10000000001XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MissingDispenseAmount)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=1XYZ";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(DispenseKeyAtEnd)
        {
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MultipleDispenseAmounts)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=100.00ABC,DISPENSE2=100.00XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(DuplicateDispenseAmounts)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=100.00ABC,DISPENSE1=100.00XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MultipleDispenseAmountsOutOfOrder)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE2=100.00ABC,DISPENSE1=100.00XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MultipleDispenseAmountsNotSequential)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=100.00ABC,DISPENSE3=100.00XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MultipleDispenseAmountsOutOfOrderNotSequential)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE2=100.00ABC,DISPENSE1=100.00XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(ShortDispenseValue)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=1AB,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(ShortCurrency)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=11AB,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MissingDispenseValueValue)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=.1ABC,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(MissingFraction)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=1.ABC,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        TEST_METHOD(FractionTooLarge)
        {
            // Valid but unsupported. 
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=1.4294967296XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            auto result = ParseDispenseToken(testToken, sizeof(testToken));
            AssertCleanFail(result);
        }
        void AssertCleanFail(bool result)
        {
            auto values = GetDispenseKeyValues();

            Assert::IsFalse(result);
            Assert::AreEqual(unsigned long(0), values->Value);
            Assert::AreEqual(unsigned long(0), values->Fraction);
            Assert::AreEqual(std::string("   "), std::string(values->Currency, 3));
        }

    };

    /// <summary>
    /// Tests around tracking tokens across commands
    /// </summary>
    TEST_CLASS(TokenTrackingTest)
    {
    public: 
        // Can use the same token multiple times. 
        TEST_METHOD(RepeatToken)
        {
            InvalidateToken(); // Reset before we start

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = ValidateToken(testToken, sizeof(testToken));

            Assert::IsTrue(result);
            // ExpectedHMAC should have been reset when CheckHMAC was called. 
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string("NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256="), LastToken);
        }

        // Can't use multiple tokens with the same nonce.  
        TEST_METHOD(DifferentToken)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F3";
            char const testToken2[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F3";
            result = ValidateToken(testToken2, sizeof(testToken2));

            Assert::IsFalse(result);
            // ExpectedHMAC should have been reset when CheckHMAC was called. 
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string("NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256="), LastToken);
        }

        // Can use a new tokens after resetting the nonce.  
        TEST_METHOD(NewNonceToken)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = ValidateToken(testToken, sizeof(testToken));
            Assert::IsTrue(result);

            result = InvalidateToken();
            Assert::IsTrue(result);
            Assert::AreEqual(std::string(""), CurrentNonce);
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string(""), LastToken);

            CurrentNonce = "99";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3FFFF";
            char const testToken2[] = "NONCE=99,TOKENFORMAT=1,TOKENLENGTH=0165,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3FFFF";
            result = ValidateToken(testToken2, sizeof(testToken2));

            Assert::IsTrue(result);
            // ExpectedHMAC should have been reset when CheckHMAC was called. 
            Assert::AreEqual(std::string(""), ExpectedHMAC);
            Assert::AreEqual(std::string("NONCE=99,TOKENFORMAT=1,TOKENLENGTH=0165,HMACSHA256="), LastToken);
        }
    };

    /// <summary>
    /// Tests around tracking the total value against a dispense token
    /// </summary>
    TEST_CLASS(DispenserValueTrackingTest)
    {
    public:
        TEST_METHOD(EqualAmount)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);

            auto values = GetDispenseKeyValues();
            Assert::IsTrue(result);
            Assert::AreEqual(unsigned long(123), values->Value);
            Assert::AreEqual(unsigned long(678), values->Fraction);
            Assert::AreEqual(std::string("XYZ"), std::string(values->Currency, 3));

            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);

            CurrentNonce = "2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 1, 0, "XYZ");
            Assert::IsFalse(result);

        }

        TEST_METHOD(MultipleDispenseExactValue)
        {
            InvalidateToken();

            static char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            bool result = false;
            DispenseKeyValues_t const *values = NULL;


            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 100, 000, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 100, 000, "XYZ");
            Assert::IsTrue(result);


            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),   0, 678, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken),   0, 678, "XYZ");
            Assert::IsTrue(result);

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),  20, 000, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken),  20, 000, "XYZ");
            Assert::IsTrue(result);

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),   3, 000, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken),   3, 000, "XYZ");
            Assert::IsTrue(result);

            CurrentNonce = "2"; // Token used up. Move to next nonce. 
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),   1, 000, "XYZ");
            Assert::IsFalse(result);
        }
        TEST_METHOD(MultipleDispenseTooMuch)
        {
            InvalidateToken();

            static char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            bool result = false;
            DispenseKeyValues_t const *values = NULL;

            SetLastDispenseIDResult = true;

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 100, 000, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 100, 000, "XYZ");
            Assert::IsTrue(result);


            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),   0, 678, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken),   0, 678, "XYZ");
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),  20, 000, "XYZ");
            Assert::IsTrue(result);
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken),  20, 000, "XYZ");
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken),   4, 000, "XYZ");
            Assert::IsFalse(result);
        }

        TEST_METHOD(NormalDispenseSequence)
        {
            bool result = false;
            result = InvalidateToken();
            Assert::IsTrue(result);

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);
            // Actual dispense here
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);

            CurrentNonce = "2";
            ExpectedHMAC = "88885612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F38888";
            char const testToken2[] = "NONCE=2,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=500.000XYZ,HMACSHA256=88885612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F38888";
            result = AuthoriseDispenseAgainstToken(testToken2, sizeof(testToken2), 500, 000, "XYZ");
            Assert::IsTrue(result);
            // Actual dispense here
            result = ConfirmDispenseAgainstToken(testToken2, sizeof(testToken2), 500, 000, "XYZ");
            Assert::IsTrue(result);
        }
        TEST_METHOD(RetryDispenseSequence)
        {
            bool result = false;
            result = InvalidateToken();
            Assert::IsTrue(result);

            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);
            // Failed dispense. Don't confirm

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);
            // Second attempt to dispense here
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);
        }
        TEST_METHOD(RetryPartialDispense)
        {
            bool result = false;
            result = InvalidateToken();
            Assert::IsTrue(result);

            SetLastDispenseIDResult = true;
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            Assert::IsTrue(result);
            // Dispense partially failed. Confirm dispensed amount
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 100, 678, "XYZ");
            Assert::IsTrue(result);

            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 23, 0, "XYZ");
            Assert::IsTrue(result);
            // Second attempt to dispense here
            result = ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 23, 0, "XYZ");
            Assert::IsTrue(result);
        }

        TEST_METHOD(ZeroValueToken)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=000.000XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 1, 0, "XYZ");
            Assert::IsFalse(result);

            auto values = GetDispenseKeyValues();
            Assert::AreEqual(unsigned long(0), values->Value);
            Assert::AreEqual(unsigned long(0), values->Fraction);
            Assert::AreEqual(std::string("   "), std::string(values->Currency, 3));
        }

        TEST_METHOD(InvalidCurrency)
        {
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            auto result = AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "ABC");
            Assert::IsFalse(result);

            auto values = GetDispenseKeyValues();
            Assert::AreEqual(unsigned long(0), values->Value);
            Assert::AreEqual(unsigned long(0), values->Fraction);
            Assert::AreEqual(std::string("   "), std::string(values->Currency, 3));
        }
    };

    /// <summary>
    /// Tests around getting a present status response token
    /// </summary>
    TEST_CLASS(GetPresentStatusTokenTest)
    {
        /// <summary>
        /// Reset all the responses that will be used when constructing the PresentStatus token to good values
        /// </summary>
        void ResetAllResponsesToGood()
        {
            SetLastDispenseIDResult = true;
            GetLastDispenseIDResult = true;
            GetLastDispenseIDKnown = true;

            // DISPENSEID
            // We need to set the last dispense hmac first, so that it can be used as the dispense id. 
            CurrentNonce = "1";
            ExpectedHMAC = "CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";
            char const testToken[] = "NONCE=1,TOKENFORMAT=1,TOKENLENGTH=0164,DISPENSE1=123.678XYZ,HMACSHA256=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2";

            AuthoriseDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");
            ConfirmDispenseAgainstToken(testToken, sizeof(testToken), 123, 678, "XYZ");

            // DISPENSED1
            GetLastDispenseResult = true;
            LastDispensedUnit = 12;
            LastDispensedSubUnit = 34;
            LastDispencedCurrency = "EUR";

            // PRESENTED1
            GetLastDispensePresentedResult = true;
            LastDispensePresented = true;

            // PRESENTEDAMOUNT1
            GetLastPresentedAmountResult = true;
            LastPresentedAmountUnit = 50;
            LastPresentedAmountSubUnit = 40;
            LastPresentedAmountCurrency = "USD";

            // RETRACTED1
            GetLastDispenseRetractedResult = true;
            LastDispenseRetracted = true;

            // RETRACTEDAMOUNT1
            GetLastRetractedAmountResult = true;
            LastRetractedAmountKnown = true;
            LastRetractedAmountUnit = 123;
            LastRetractedAmountSubUnit = 45;
            LastRetractedAmountCurrency = "EUR";

            // HMACSHA256
            NewHMACResult = true;
            NewHMACString = "55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83";
        }
        TEST_METHOD(GoodGetPresentStatusToken)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0296,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=12.34EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.40USD,RETRACTED1=YES,RETRACTEDAMOUNT1=123.45EUR,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;
            // Sample token from the XFS specification
            // NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0268,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=50.00EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.00EUR,RETRACTED1=NO,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83
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

            char const* responceNonce = "1414";

            ResetAllResponsesToGood();

            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsTrue(result);
            Assert::AreEqual(expectedToken, string(responceToken));
        }
        // Nonce tests
        TEST_METHOD(ResponceNonceNull)
        {
            char const* responceNonce = NULL;
            char const* responceToken = (char*)1; 
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(ResponceNonceEmpty)
        {
            char const* responceNonce = "";
            char const* responceToken = (char*)1;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(ResponceNonceTooLong)
        {
            // Set an arbitary maximum length on the nonce - 100 characters
            auto maxNonceLen = 100;
            char const* responceNonce = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            char const* responceToken = (char*)1;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        // DispenseID tests
        TEST_METHOD(GetLastDispenseIDResultFalse)
        {
            ResetAllResponsesToGood();
        
            // DISPENSEID
            GetLastDispenseIDResult = false;
                
            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);
        
            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(GetLastDispenseIDResultUnknown)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0220,DISPENSED1=12.34EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.40USD,RETRACTED1=YES,RETRACTEDAMOUNT1=123.45EUR,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;
            char const* responceNonce = "1414";

            ResetAllResponsesToGood();

            GetLastDispenseIDKnown = false;

            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsTrue(result);
            Assert::AreEqual(expectedToken, string(responceToken));
        }
        // Dispensed (amount) tests
        TEST_METHOD(GetLastDispenseAmountResultFalse)
        {
            ResetAllResponsesToGood(); 

            GetLastDispenseResult = false;
            LastDispensedUnit = 12;
            LastDispensedSubUnit = 34;
            LastDispencedCurrency = "EUR";

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }        
        // Presented flag tests
        TEST_METHOD(GetLastDispensePresentedResultFalse)
        {
            ResetAllResponsesToGood(); 

            GetLastPresentedAmountResult = false;
            LastPresentedAmountUnit = 99;
            LastPresentedAmountSubUnit = 99;
            LastPresentedAmountCurrency = "XXX";

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(GetLastDispensePresentedNo)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0295,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=12.34EUR,PRESENTED1=NO,PRESENTEDAMOUNT1=50.40USD,RETRACTED1=YES,RETRACTEDAMOUNT1=123.45EUR,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;

            ResetAllResponsesToGood();
            GetLastDispensePresentedResult = true;
            LastDispensePresented = false; 

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsTrue(result);
            Assert::AreEqual(expectedToken, string(responceToken));
        }
        // PresentedAmount tests
        TEST_METHOD(GetLastPresentedAmountResultFalse)
        {
            ResetAllResponsesToGood(); 

            GetLastPresentedAmountResult = false;
            LastDispensedUnit = 99;
            LastDispensedSubUnit = 99;
            LastDispencedCurrency = "XXX";

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        // Retracted flag tests
        TEST_METHOD(GetLastRetractedResultFalse)
        {
            ResetAllResponsesToGood();

            GetLastDispenseRetractedResult = false;
            LastDispenseRetracted = true;

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(GetLastDispenseRetractedNo)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0295,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=12.34EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.40USD,RETRACTED1=NO,RETRACTEDAMOUNT1=123.45EUR,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;

            ResetAllResponsesToGood();
            LastDispenseRetracted = false;

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsTrue(result);
            Assert::AreEqual(expectedToken, string(responceToken));
        }
        // Retracted amount tests
        TEST_METHOD(GetLastRetractedAmountResultFalse)
        {
            ResetAllResponsesToGood(); 

            GetLastRetractedAmountResult = false;
            LastRetractedAmountUnit = 99;
            LastRetractedAmountSubUnit = 99;
            LastRetractedAmountCurrency = "XXX";

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        TEST_METHOD(GetLastRetractedAmountUnknown)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0288,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=12.34EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.40USD,RETRACTED1=YES,RETRACTEDAMOUNT1=?,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;

            ResetAllResponsesToGood();
            LastRetractedAmountKnown = false;

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsTrue(result);
            Assert::AreEqual(expectedToken, string(responceToken));
        }
        // HMACSHA256 tests
        TEST_METHOD(NewHMACFalse)
        {
            ResetAllResponsesToGood();

            NewHMACResult = false; 

            char const* responceNonce = "1414";
            char const* responceToken = NULL;
            auto result = GetPresentStatusToken(responceNonce, &responceToken);

            Assert::IsFalse(result);
            Assert::IsNull(responceToken);
        }
        // General tests
        TEST_METHOD(ResponceTokenBufferNull)
        {
            string expectedToken = "NONCE=1414,TOKENFORMAT=1,TOKENLENGTH=0268,DISPENSEID=CB735612FD6141213C2827FB5A6A4F4846D7A7347B15434916FEA6AC16F3D2F2,DISPENSED1=50.00EUR,PRESENTED1=YES,PRESENTEDAMOUNT1=50.00EUR,RETRACTED1=NO,HMACSHA256=55D123E9EE64F0CC3D1CD4F953348B441E521BBACCD6998C6F51D645D71E6C83"s;

            char const* responceNonce = "1";
            auto result = GetPresentStatusToken(responceNonce, NULL);

            Assert::IsFalse(result);
        }

    };
}
