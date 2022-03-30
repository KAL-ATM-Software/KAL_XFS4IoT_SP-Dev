/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<byte> rawData = System.Text.Encoding.UTF8.GetBytes("123456789").ToList();
            var payload = new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, "OK", null, 
                new (CardDataStatusEnum.Ok, rawData), 
                new (CardDataStatusEnum.DataMissing, rawData), 
                new (CardDataStatusEnum.DataInvalid, rawData));
            var response = new ReadRawDataCompletion(123456, payload);

            IsNotNull(response.Payload.Track1);
            IsNotNull(response.Payload.Track2);
            IsNotNull(response.Payload.Track3);
            AreEqual(rawData, response.Payload.Track1.Data);
            AreEqual(CardDataStatusEnum.DataInvalid, response.Payload.Track3.Status);
        }

        [TestMethod]
        public void SerialiseString()
        {
            List<byte> rawData = System.Text.Encoding.UTF8.GetBytes("123456789").ToList();
            var payload = new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, "OK", null,
                new (CardDataStatusEnum.Ok, rawData),
                new (CardDataStatusEnum.DataMissing, rawData),
                new (CardDataStatusEnum.DataInvalid, rawData));
            var response = new ReadRawDataCompletion(123456, payload);

            string res = response.Serialise();
            
            AreEqual(@"{""header"":{""name"":""CardReader.ReadRawData"",""requestId"":123456,""type"":""completion""},""payload"":{""track1"":{""status"":""ok"",""data"":""MTIzNDU2Nzg5""},""track2"":{""status"":""dataMissing"",""data"":""MTIzNDU2Nzg5""},""track3"":{""status"":""dataInvalid"",""data"":""MTIzNDU2Nzg5""},""completionCode"":""success"",""errorDescription"":""OK""}}", res);
        }
    }
}
