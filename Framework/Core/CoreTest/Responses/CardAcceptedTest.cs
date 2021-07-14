/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Completions;
using XFS4IoT.CardReader;

namespace XFS4IoTCoreTest.Response
{
    using static Assert; 

    [TestClass]
    public class ResponseSerialisationTest
    {
        [TestMethod]
        public void Constructor()
        {
            var payload = new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, "OK", null, 
                new (CardDataStatusEnum.Ok, "123456789"), 
                new (CardDataStatusEnum.DataMissing, "123456789"), 
                new (CardDataStatusEnum.DataInvalid, "123456789"));
            var response = new ReadRawDataCompletion(123456, payload);

            IsNotNull(response.Payload.Track1);
            IsNotNull(response.Payload.Track2);
            IsNotNull(response.Payload.Track3);
            AreEqual("123456789", response.Payload.Track1.Data);
            AreEqual(CardDataStatusEnum.DataInvalid, response.Payload.Track3.Status);
        }

        [TestMethod]
        public void SerialiseString()
        {
            var payload = new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, "OK", null,
                new (CardDataStatusEnum.Ok, "123456789"),
                new (CardDataStatusEnum.DataMissing, "123456789"),
                new (CardDataStatusEnum.DataInvalid, "123456789"));
            var response = new ReadRawDataCompletion(123456, payload);

            string res = response.Serialise();

            AreEqual(@"{""payload"":{""track1"":{""status"":""ok"",""data"":""123456789""},""track2"":{""status"":""dataMissing"",""data"":""123456789""},""track3"":{""status"":""dataInvalid"",""data"":""123456789""},""completionCode"":""success"",""errorDescription"":""OK""},""header"":{""name"":""CardReader.ReadRawData"",""requestId"":123456,""type"":""completion""}}", res);
        }
    }
}
