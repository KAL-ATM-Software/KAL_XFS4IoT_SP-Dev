/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SetMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = SetMedia
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "Printer.SetMedia")]
    public sealed class SetMediaCommand : Command<SetMediaCommand.PayloadData>
    {
        public SetMediaCommand(int RequestId, SetMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Name = null, MediaClass Media = null)
                : base()
            {
                this.Name = Name;
                this.Media = Media;
            }

            /// <summary>
            /// The media definition name.
            /// <example>My Media</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            [DataContract]
            public sealed class MediaClass
            {
                public MediaClass(MediaTypeEnum? MediaType = null, string Source = null, UnitClass Unit = null, SizeClass Size = null, AreaClass PrintArea = null, AreaClass Restricted = null, FoldEnum? Fold = null, int? Staggering = null, int? Page = null, int? Lines = null)
                {
                    this.MediaType = MediaType;
                    this.Source = Source;
                    this.Unit = Unit;
                    this.Size = Size;
                    this.PrintArea = PrintArea;
                    this.Restricted = Restricted;
                    this.Fold = Fold;
                    this.Staggering = Staggering;
                    this.Page = Page;
                    this.Lines = Lines;
                }

                public enum MediaTypeEnum
                {
                    Generic,
                    Passbook,
                    Multipart
                }

                /// <summary>
                /// Specifies the type of media as one of the following:
                /// 
                /// * ```generic``` - The media is generic, i.e., a single sheet.
                /// * ```passbook``` - The media is a passbook.
                /// * ```multipart``` - The media is a multi-part.
                /// </summary>
                [DataMember(Name = "mediaType")]
                public MediaTypeEnum? MediaType { get; init; }

                /// <summary>
                /// Specifies the paper source to use when printing forms using this media definition as one of the following:
                /// 
                /// * ```any``` - Any paper source.
                /// * ```upper``` - The only paper source or the upper paper source, if there is more than one paper supply.
                /// * ```lower``` - The lower paper source.
                /// * ```external``` - The external paper source.
                /// * ```aux``` - The auxiliary paper source.
                /// * ```aux2``` - The second auxiliary paper source.
                /// * ```park``` - The park paper source.
                /// * ```[paper source identifier]``` - The vendor specific paper source.
                /// <example>lower</example>
                /// </summary>
                [DataMember(Name = "source")]
                [DataTypes(Pattern = @"^any$|^upper$|^lower$|^external$|^aux$|^aux2$|^park$|^[a-zA-Z]([a-zA-Z0-9]*)$")]
                public string Source { get; init; }

                [DataMember(Name = "unit")]
                public UnitClass Unit { get; init; }

                [DataMember(Name = "size")]
                public SizeClass Size { get; init; }

                /// <summary>
                /// Printable area relative to top left corner of physical media.
                /// </summary>
                [DataMember(Name = "printArea")]
                public AreaClass PrintArea { get; init; }

                /// <summary>
                /// Restricted area relative to top left corner of physical media.
                /// </summary>
                [DataMember(Name = "restricted")]
                public AreaClass Restricted { get; init; }

                public enum FoldEnum
                {
                    Horizontal,
                    Vertical
                }

                /// <summary>
                /// Specified the type of fold for media of type *passbook* as one of the following:
                /// 
                /// * ```horizontal``` - Passbook has a horizontal fold.
                /// * ```vertical``` - Passbook has a vertical fold.
                /// <example>vertical</example>
                /// </summary>
                [DataMember(Name = "fold")]
                public FoldEnum? Fold { get; init; }

                /// <summary>
                /// Specifies the staggering from the top in terms of the base vertical resolution for media of type
                /// *passbook*.
                /// <example>2</example>
                /// </summary>
                [DataMember(Name = "staggering")]
                [DataTypes(Minimum = 0)]
                public int? Staggering { get; init; }

                /// <summary>
                /// Specifies the number of pages in media of type *passbook*. This can be null if not applicable.
                /// <example>20</example>
                /// </summary>
                [DataMember(Name = "page")]
                [DataTypes(Minimum = 1)]
                public int? Page { get; init; }

                /// <summary>
                /// Specifies the number of printable lines. This can be null if not applicable.
                /// <example>15</example>
                /// </summary>
                [DataMember(Name = "lines")]
                [DataTypes(Minimum = 1)]
                public int? Lines { get; init; }

            }

            /// <summary>
            /// The details of the media definition. May be null if the media definition is to be deleted.
            /// </summary>
            [DataMember(Name = "media")]
            public MediaClass Media { get; init; }

        }
    }
}
