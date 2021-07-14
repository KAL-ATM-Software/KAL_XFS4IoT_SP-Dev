/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoT
{
    /// <summary>
    /// Indicate that data types represented in the YAML schema
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DataTypesAttribute : Attribute
    {
        /// <summary>
        /// The reglar expression is supported for type string object
        /// </summary>
        public string Pattern { get; set; } = null;

        /// <summary>
        /// Minimum length or Maximum length of the string object
        /// </summary>
        public uint? MinLength { get; set; } = null;
        public uint? MaxLength { get; set; } = null;

        /// <summary>
        /// The Sensitive flag is set to true if the property contains customer senstive information, otherwise false and supported for only type string object
        /// The data is set in the angle brackes if the data of the property to be masked.
        /// </summary>
        public bool Sensitive { get; set; } = false;

        /// <summary>
        /// Use the Minimum and Maximum to specify the range of possible values for type int
        /// </summary>
        public int Minimum { get; set; } = -1;
        public int Maximum { get; set; } = -1;
    }
}