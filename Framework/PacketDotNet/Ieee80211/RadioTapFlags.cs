﻿using System;

namespace PacketDotNet.Ieee80211
{
    namespace Ieee80211
    {
        /// <summary>
        ///     Radio tap flags
        /// </summary>
        [Flags]
        public enum RadioTapFlags
        {
            /// <summary>
            ///     sent/received during cfp
            /// </summary>
            CFP = 0x01,

            /// <summary>
            ///     sent/received with short preamble
            /// </summary>
            ShortPreamble = 0x02,

            /// <summary>
            ///     sent/received with WEP encryption
            /// </summary>
            WepEncrypted = 0x04,

            /// <summary>
            ///     sent/received with fragmentation
            /// </summary>
            Fragmentation = 0x08,

            /// <summary>
            ///     frame includes FCS
            /// </summary>
            FcsIncludedInFrame = 0x10,

            /// <summary>
            ///     frame includes padding to the 32 bit boundary between the 802.11 header and the payload
            /// </summary>
            PostFramePadding = 0x20,

            /// <summary>
            ///     fFrame failed the fcs check
            /// </summary>
            FailedFcsCheck = 0x40
        };
    }
}