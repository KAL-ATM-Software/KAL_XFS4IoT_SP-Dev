/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;
using System;
using XFS4IoT;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTCoreTest.Response
{
    using static Assert;

    [TestClass]
    public class UnserialiseTests
    {
        private readonly AssemblyName AssemblyName;

        public UnserialiseTests()
        {
            AssemblyName = Assembly.GetAssembly(typeof(UnserialiseTests))?.GetName();
        }

        [TestMethod]
        public void UnserialiseStringToObject()
        {
            var ReadCardJSON = @"{""payload"":{""track1"":{""status"":""ok"",""data"":""123456789""},""track2"":{""status"":""dataMissing"",""data"":""123456789""},""track3"":{""status"":""dataInvalid"",""data"":""123456789""},""completionCode"":""success"",""errorDescription"":""OK""},""headers"":{""name"":""CardReader.ReadRawData"",""requestId"":123456,""type"":""completion""}}";

            var assemblyName = Assembly.GetAssembly(typeof(ReadRawDataCompletion))?.GetName();
            IsNotNull(assemblyName);

            var decoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Response, assemblyName)
            {
                { typeof(ReadRawDataCompletion) }
            };

            bool rc = decoder.TryUnserialise(ReadCardJSON, out object resultMessage);

            IsTrue(rc);
            IsNotNull(resultMessage);

            Completion<ReadRawDataCompletion.PayloadData> result = resultMessage as Completion<ReadRawDataCompletion.PayloadData> ?? throw new Exception();

            IsNotNull(result);

            IsInstanceOfType(result, typeof(ReadRawDataCompletion));
            ReadRawDataCompletion readCardCompletion = result as ReadRawDataCompletion;
            IsNotNull(readCardCompletion);
            IsNotNull(readCardCompletion.Payload);
            ReadRawDataCompletion.PayloadData readCardPayload = readCardCompletion.Payload as ReadRawDataCompletion.PayloadData;
            IsNotNull(readCardPayload);
            IsNotNull(readCardPayload.Track1);
            IsNotNull(readCardPayload.Track2);
            IsNotNull(readCardPayload.Track3);
        }

        [TestMethod]
        public void UnserialiseStringToObjectNoHeader()
        {
            var AcceptCardJSON = @"{
                ""payload"":{
                    ""timeout"":5000,
                    ""track1"":true,
                    ""track2"": true,
                    ""track3"":true,
                    ""chip"":true,
                    ""security"":true,
                    ""fluxInactive"":true,
                    ""watermark"":true,
                    ""memoryChip"":true,
                    ""track1Front"":true,
                    ""frontImage"":true,
                    ""backImage"":true,
                    ""track1JIS"":true,
                    ""track3JIS"":true,
                    ""ddi"":true
            }";

            var assemblyName = Assembly.GetAssembly(typeof(ReadRawDataCommand))?.GetName();
            IsNotNull(assemblyName);

            var decoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Command, assemblyName)
            {
                { typeof(ReadRawDataCommand) }
            };

            bool rc = decoder.TryUnserialise(AcceptCardJSON, out object result);
            IsFalse(rc);
            AreEqual(null, result);
        }

        [TestMethod]
        public void UnserialiseStringToObjectNotJSON()
        {
            var AcceptCardJSON = @"Not JSON";
            var assemblyName = Assembly.GetAssembly(typeof(ReadRawDataCommand))?.GetName();
            IsNotNull(assemblyName);

            var decoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Command, assemblyName)
            {
                { typeof(ReadRawDataCommand) }
            };

            bool rc = decoder.TryUnserialise(AcceptCardJSON, out object result);
            IsFalse(rc);
            AreEqual(null, result);
        }
    }
}
