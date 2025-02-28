/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT;

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
            var payload = new ReadRawDataCompletion.PayloadData( 
                Track1: new (null, rawData), 
                Track2: new (CardDataStatusEnum.DataMissing, rawData), 
                Track3: new (CardDataStatusEnum.DataInvalid, rawData));
            var response = new ReadRawDataCompletion(123456, payload, MessageHeader.CompletionCodeEnum.Success, "OK");

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
            var payload = new ReadRawDataCompletion.PayloadData(
                Track1: new (null, rawData),
                Track2: new (CardDataStatusEnum.DataMissing, rawData),
                Track3: new (CardDataStatusEnum.DataInvalid, rawData));
            var response = new ReadRawDataCompletion(123456, payload, MessageHeader.CompletionCodeEnum.Success, "OK");

            string res = response.Serialise();
            
            AreEqual(@"{""header"":{""name"":""CardReader.ReadRawData"",""requestId"":123456,""type"":""completion"",""version"":""3.0"",""completionCode"":""success"",""errorDescription"":""OK""},""payload"":{""track1"":{""data"":""MTIzNDU2Nzg5""},""track2"":{""status"":""dataMissing"",""data"":""MTIzNDU2Nzg5""},""track3"":{""status"":""dataInvalid"",""data"":""MTIzNDU2Nzg5""}}}", res);
        }
    }
}
