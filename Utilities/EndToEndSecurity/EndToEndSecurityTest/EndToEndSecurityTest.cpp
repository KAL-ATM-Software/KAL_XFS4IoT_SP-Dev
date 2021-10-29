/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

#include "pch.h"
#include "CppUnitTest.h"
#include "EndToEndSecurity.h"
#include <mutex>

using namespace std; 
using namespace Microsoft::VisualStudio::CppUnitTestFramework;

mutex threadSafe; 

extern "C" void Log(char const* const Message)
{
    Logger::WriteMessage( Message );
};

extern "C" void FatalError(char const* const Message)
{
    using namespace Microsoft::VisualStudio::CppUnitTestFramework;
    Assert::Fail(ToString(Message).c_str());
}

static std::string CurrentNonce = "";
extern "C" void NewNonce(char const ** OutNonce)
{
    // Value will be set by tests - not the normal nonce behaviour! 
    *OutNonce = CurrentNonce.c_str(); 
}

extern "C" void ClearNonce()
{
    CurrentNonce = "";
}

extern "C" bool CompareNonce(char const* const CommandNonce, unsigned long NonceLength )
{
    std::string nonce = std::string(CommandNonce, NonceLength);
    return nonce == CurrentNonce;
}

static std::string ExpectedHMAC = ""; 
static std::string LastToken = ""; 
extern "C" bool CheckHMAC(char const* const Token, unsigned int TokenLength, unsigned char const* const TokenHMAC)
{
    Assert::IsNotNull(Token);
    Assert::IsNotNull(TokenHMAC);

    static char TokenHMACString[65];
    for (unsigned int i = 0; i < 32; i++)
    {
        sprintf_s((TokenHMACString+(i*2)),3, "%1X%1X", (TokenHMAC[i] & 0xF0) >> 4, TokenHMAC[i] & 0x0F);
    }

    bool result = std::string(TokenHMACString,64) == ExpectedHMAC; 

    ExpectedHMAC = ""; 
    LastToken = std::string(Token, TokenLength);

    return result;
}

namespace EndToEndSecurityTest
{
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

    TEST_CLASS(DispenserTest)
    {
    public: 
        TEST_METHOD(NullToken)
        {
            lock_guard<mutex> guard(threadSafe);

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
}
