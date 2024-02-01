/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Events;

namespace XFS4IoTCoreTest.Command
{
    using static Assert;

    [TestClass]
    public class CommandSerialisationTest
    {
        [TestMethod]
        public void Constructor()
        {
            var command = new ReadRawDataCommand(123456, new ReadRawDataCommand.PayloadData(true, true, true, true, true, true, true, true, true, true, true, true, true, true), 1000);

            Assert.AreEqual(1000, command.Header.Timeout);
        }

        [TestMethod]
        public void SerialiseString()
        {
            var command = new ReadRawDataCommand(123456, new ReadRawDataCommand.PayloadData(true, true, true, true, true, true, true, true, true, true, true, true, true, true), 5000);

            string result = command.Serialise();

            AreEqual(@"{""header"":{""name"":""CardReader.ReadRawData"",""requestId"":123456,""type"":""command"",""version"":""2.0"",""timeout"":5000},""payload"":{""track1"":true,""track2"":true,""track3"":true,""chip"":true,""security"":true,""fluxInactive"":true,""watermark"":true,""memoryChip"":true,""track1Front"":true,""frontImage"":true,""backImage"":true,""track1JIS"":true,""track3JIS"":true,""ddi"":true}}", result);

        }

        [TestMethod]
        public void SerialiseEvent()
        {
            var mediaInserted = new MediaInsertedEvent(123456);

            string result = mediaInserted.Serialise();
            AreEqual(@"{""header"":{""name"":""CardReader.MediaInsertedEvent"",""requestId"":123456,""type"":""event"",""version"":""2.0""}}", result);
        }
    }
}
