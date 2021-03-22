/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            var command = new ReadRawDataCommand(Guid.NewGuid().ToString(), new ReadRawDataCommand.PayloadData(1000, true, true, true, true, true, true, true, true, true, true, true, true, true, true));

            Assert.AreEqual(1000, command.Payload.Timeout);
        }

        [TestMethod]
        public void SerialiseString()
        {
            var command = new ReadRawDataCommand("ee6d592b-483c-4c22-98ef-1070e290bf4f", new ReadRawDataCommand.PayloadData(5000, true, true, true, true, true, true, true, true, true, true, true, true, true, true));

            string result = command.Serialise();

            AreEqual(@"{""payload"":{""track1"":true,""track2"":true,""track3"":true,""chip"":true,""security"":true,""fluxInactive"":true,""watermark"":true,""memoryChip"":true,""track1Front"":true,""frontImage"":true,""backImage"":true,""track1JIS"":true,""track3JIS"":true,""ddi"":true,""timeout"":5000},""headers"":{""name"":""CardReader.ReadRawData"",""requestId"":""ee6d592b-483c-4c22-98ef-1070e290bf4f"",""type"":""command""}}", result);

        }

        [TestMethod]
        public void SerialiseEvent()
        {
            var mediaInserted = new MediaInsertedEvent("ee6d592b-483c-4c22-99ef-1070e290bf4f");

            string result = mediaInserted.Serialise();
            AreEqual(@"{""payload"":{},""headers"":{""name"":""CardReader.MediaInsertedEvent"",""requestId"":""ee6d592b-483c-4c22-99ef-1070e290bf4f"",""type"":""event""}}", result);
        }
    }
}
