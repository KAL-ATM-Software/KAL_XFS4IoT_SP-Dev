/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * WriteRawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = WriteRawData
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "CardReader.WriteRawData")]
    public sealed class WriteRawDataCommand : Command<WriteRawDataCommand.PayloadData>
    {
        public WriteRawDataCommand(int RequestId, WriteRawDataCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Track1Class Track1 = null, Track2Class Track2 = null, Track3Class Track3 = null, Track1FrontClass Track1Front = null, Track1JISClass Track1JIS = null, Track3JISClass Track3JIS = null, AdditionalPropertiesClass AdditionalProperties = null)
                : base()
            {
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Track1Front = Track1Front;
                this.Track1JIS = Track1JIS;
                this.Track3JIS = Track3JIS;
                this.AdditionalProperties = AdditionalProperties;
            }

            [DataContract]
            public sealed class Track1Class
            {
                public Track1Class(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to track 1. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track1")]
            public Track1Class Track1 { get; init; }

            [DataContract]
            public sealed class Track2Class
            {
                public Track2Class(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to track 2. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track2")]
            public Track2Class Track2 { get; init; }

            [DataContract]
            public sealed class Track3Class
            {
                public Track3Class(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to track 3. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track3")]
            public Track3Class Track3 { get; init; }

            [DataContract]
            public sealed class Track1FrontClass
            {
                public Track1FrontClass(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to the front track 1. In some countries this track
            /// is known as JIS II track. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track1Front")]
            public Track1FrontClass Track1Front { get; init; }

            [DataContract]
            public sealed class Track1JISClass
            {
                public Track1JISClass(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to JIS I track 1 (8bits/char). This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track1JIS")]
            public Track1JISClass Track1JIS { get; init; }

            [DataContract]
            public sealed class Track3JISClass
            {
                public Track3JISClass(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to JIS I track 3 (8bits/char). This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "track3JIS")]
            public Track3JISClass Track3JIS { get; init; }

            [DataContract]
            public sealed class AdditionalPropertiesClass
            {
                public AdditionalPropertiesClass(List<byte> Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                /// <summary>
                /// Base64 encoded representation of the data.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Data { get; init; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written. If this property is null,
                /// the service will determine whether low or high coercivity is to be used.
                /// 
                /// Specifies the write method as one of the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; init; }

            }

            /// <summary>
            /// Specifies data is to be written to vendor specific track. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "additionalProperties")]
            public AdditionalPropertiesClass AdditionalProperties { get; init; }

        }
    }
}
