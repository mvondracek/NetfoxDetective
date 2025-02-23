using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using log4net;
using PacketDotNet.MiscUtil.Conversion;
using PacketDotNet.Utils;

namespace PacketDotNet
{
    /// <summary>
    ///     IPv4 packet
    ///     See http://en.wikipedia.org/wiki/IPv4 for into
    /// </summary>
    public class IPv4Packet : IpPacket
    {
#if DEBUG
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
#else
    // NOTE: No need to warn about lack of use, the compiler won't
    //       put any calls to 'log' here but we need 'log' to exist to compile
#pragma warning disable 0169, 0649
        private static readonly ILogInactive log;
#pragma warning restore 0169, 0649
#endif

        /// <value>
        ///     Number of bytes in the smallest valid ipv4 packet
        /// </value>
        public const int HeaderMinimumLength = 20;

        /// <summary>
        ///     Type of service code constants for IP. Type of service describes
        ///     how a packet should be handled.
        ///     <p>
        ///         TOS is an 8-bit record in an IP header which contains a 3-bit
        ///         precendence field, 4 TOS bit fields and a 0 bit.
        ///     </p>
        ///     <p>
        ///         The following constants are bit masks which can be logically and'ed
        ///         with the 8-bit IP TOS field to determine what type of service is set.
        ///     </p>
        ///     <p>
        ///         Taken from TCP/IP Illustrated V1 by Richard Stevens, p34.
        ///     </p>
        /// </summary>
        public struct TypesOfService_Fields
        {
#pragma warning disable 1591
            public static readonly int MINIMIZE_DELAY = 0x10;
            public static readonly int MAXIMIZE_THROUGHPUT = 0x08;
            public static readonly int MAXIMIZE_RELIABILITY = 0x04;
            public static readonly int MINIMIZE_MONETARY_COST = 0x02;
            public static readonly int UNUSED = 0x01;
#pragma warning restore 1591
        }

        /// <value>
        ///     Version number of the IP protocol being used
        /// </value>
        public static IpVersion ipVersion = IpVersion.IPv4;

        /// <summary> Get the IP version code.</summary>
        public override IpVersion Version
        {
            get { return (IpVersion) ((this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition] >> 4)&0x0F); }

            set
            {
                // read the original value
                var theByte = this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition];

                // mask in the version bits
                theByte = (byte) ((theByte&0x0F)|(((byte) value << 4)&0xF0));

                // write back the modified value
                this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition] = theByte;
            }
        }

        /// <value>
        ///     Forwards compatibility IPv6.PayloadLength property
        /// </value>
        public override ushort PayloadLength
        {
            get { return (ushort) (this.TotalLength - (this.HeaderLength*4)); }

            set { this.TotalLength = value + (this.HeaderLength*4); }
        }

        /// <summary>
        ///     The IP header length field.  At most, this can be a
        ///     four-bit value.  The high order bits beyond the fourth bit
        ///     will be ignored.
        /// </summary>
        /// <param name="length">
        ///     The length of the IP header in 32-bit words.
        /// </param>
        public override int HeaderLength
        {
            get { return (this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition])&0x0F; }

            set
            {
                // read the original value
                var theByte = this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition];

                // mask in the header length bits
                theByte = (byte) ((theByte&0xF0)|(((byte) value)&0x0F));

                // write back the modified value
                this.header.Bytes[this.header.Offset + IPv4Fields.VersionAndHeaderLengthPosition] = theByte;
            }
        }

        /// <summary>
        ///     The unique ID of this IP datagram. The ID normally
        ///     increments by one each time a datagram is sent by a host.
        ///     A 16-bit unsigned integer.
        /// </summary>
        public virtual ushort Id
        {
            get { return EndianBitConverter.Big.ToUInt16(this.header.Bytes, this.header.Offset + IPv4Fields.IdPosition); }

            set { EndianBitConverter.Big.CopyBytes(value, this.header.Bytes, this.header.Offset + IPv4Fields.IdPosition); }
        }

        /// <summary>
        ///     Fragmentation offset
        ///     The offset specifies a number of octets (i.e., bytes).
        ///     A 13-bit unsigned integer.
        /// NOTE: FragmentOffset is here measured in 8 byte increments. For example Wireshark shows 'Fragment offset' in bytes, therefore its shows 8 times bigger value.
        /// </summary>
        public virtual int FragmentOffset
        {
            get
            {
                var fragmentOffsetAndFlags = EndianBitConverter.Big.ToInt16(this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);

                // mask off the high flag bits
                return (fragmentOffsetAndFlags&0x1FFF);
            }

            set
            {
                // retrieve the value
                var fragmentOffsetAndFlags = EndianBitConverter.Big.ToInt16(this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);

                // mask the fragementation offset in
                fragmentOffsetAndFlags = (short) ((fragmentOffsetAndFlags&0xE000)|(value&0x1FFF));

                EndianBitConverter.Big.CopyBytes(fragmentOffsetAndFlags, this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);
            }
        }

        /// <summary> Fetch the IP address of the host where the packet originated from.</summary>
        public override IPAddress SourceAddress
        {
            get { return GetIPAddress(AddressFamily.InterNetwork, this.header.Offset + IPv4Fields.SourcePosition, this.header.Bytes); }

            set
            {
                var address = value.GetAddressBytes();
                Array.Copy(address, 0, this.header.Bytes, this.header.Offset + IPv4Fields.SourcePosition, address.Length);
            }
        }

        /// <summary> Fetch the IP address of the host where the packet is destined.</summary>
        public override IPAddress DestinationAddress
        {
            get { return GetIPAddress(AddressFamily.InterNetwork, this.header.Offset + IPv4Fields.DestinationPosition, this.header.Bytes); }

            set
            {
                var address = value.GetAddressBytes();
                Array.Copy(address, 0, this.header.Bytes, this.header.Offset + IPv4Fields.DestinationPosition, address.Length);
            }
        }

        /// <summary> Fetch the header checksum.</summary>
        public virtual ushort Checksum
        {
            get { return EndianBitConverter.Big.ToUInt16(this.header.Bytes, this.header.Offset + IPv4Fields.ChecksumPosition); }

            set
            {
                var val = value;
                EndianBitConverter.Big.CopyBytes(val, this.header.Bytes, this.header.Offset + IPv4Fields.ChecksumPosition);
            }
        }

        /// <summary> Check if the IP packet is valid, checksum-wise.</summary>
        public virtual bool ValidChecksum
        {
            get { return this.ValidIPChecksum; }
        }

        /// <summary>
        ///     Check if the IP packet header is valid, checksum-wise.
        /// </summary>
        public bool ValidIPChecksum
        {
            get
            {
                log.Debug("");

                // first validate other information about the packet. if this stuff
                // is not true, the packet (and therefore the checksum) is invalid
                // - ip_hl >= 5 (ip_hl is the length in 4-byte words)
                if(this.Header.Length < IPv4Fields.HeaderLength)
                {
                    log.DebugFormat("invalid length, returning false");
                    return false;
                }
                var headerOnesSum = ChecksumUtils.OnesSum(this.Header);
                log.DebugFormat(HexPrinter.GetString(this.Header, 0, this.Header.Length));
                const int expectedHeaderOnesSum = 0xffff;
                var retval = (headerOnesSum == expectedHeaderOnesSum);
                log.DebugFormat("headerOnesSum: {0}, expectedHeaderOnesSum {1}, returning {2}", headerOnesSum, expectedHeaderOnesSum, retval);
                log.DebugFormat("Header.Length {0}", this.Header.Length);
                return retval;
            }
        }

        /// <summary> Fetch ascii escape sequence of the color associated with this packet type.</summary>
        public override String Color
        {
            get { return AnsiEscapeSequences.White; }
        }

        /// <summary> Fetch the type of service. </summary>
        public int DifferentiatedServices
        {
            get { return this.header.Bytes[this.header.Offset + IPv4Fields.DifferentiatedServicesPosition]; }

            set { this.header.Bytes[this.header.Offset + IPv4Fields.DifferentiatedServicesPosition] = (byte) value; }
        }

        /// <value>
        ///     Renamed to DifferentiatedServices in IPv6 but present here
        ///     for backwards compatibility
        /// </value>
        public int TypeOfService
        {
            get { return this.DifferentiatedServices; }
            set { this.DifferentiatedServices = value; }
        }

        /// <value>
        ///     The entire datagram size including header and data
        /// </value>
        public override int TotalLength
        {
            get { return EndianBitConverter.Big.ToUInt16(this.header.Bytes, this.header.Offset + IPv4Fields.TotalLengthPosition); }

            set
            {
                var theValue = (UInt16) value;
                EndianBitConverter.Big.CopyBytes(theValue, this.header.Bytes, this.header.Offset + IPv4Fields.TotalLengthPosition);
            }
        }

        /// <summary> Fetch fragment flags.</summary>
        /// <param name="flags">A 3-bit unsigned integer.</param>
        public virtual Int16 FragmentFlags
        {
            get
            {
                var fragmentOffsetAndFlags = EndianBitConverter.Big.ToInt16(this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);

                // shift off the fragment offset bits
                return (Int16) (fragmentOffsetAndFlags >> (16 - 3));
            }

            set
            {
                // retrieve the value
                var fragmentOffsetAndFlags = EndianBitConverter.Big.ToInt16(this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);

                // mask the flags in
                fragmentOffsetAndFlags = (short) ((fragmentOffsetAndFlags&0x1FFF)|((value&0x07) << (16 - 3)));

                EndianBitConverter.Big.CopyBytes(fragmentOffsetAndFlags, this.header.Bytes, this.header.Offset + IPv4Fields.FragmentOffsetAndFlagsPosition);
            }
        }

        /// <summary>
        ///     Fetch the time to live. TTL sets the upper limit on the number of
        ///     routers through which this IP datagram is allowed to pass.
        ///     Originally intended to be the number of seconds the packet lives it is now decremented
        ///     by one each time a router passes the packet on
        ///     8-bit value
        /// </summary>
        public override int TimeToLive
        {
            get { return this.header.Bytes[this.header.Offset + IPv4Fields.TtlPosition]; }

            set { this.header.Bytes[this.header.Offset + IPv4Fields.TtlPosition] = (byte) value; }
        }

        /// <summary> Fetch the code indicating the type of protocol embedded in the IP</summary>
        /// <seealso cref="IPProtocolType">
        /// </seealso>
        public override IPProtocolType Protocol
        {
            get { return (IPProtocolType) this.header.Bytes[this.header.Offset + IPv4Fields.ProtocolPosition]; }

            set { this.header.Bytes[this.header.Offset + IPv4Fields.ProtocolPosition] = (byte) value; }
        }

        /// <summary>
        ///     Calculates the IP checksum, optionally updating the IP checksum header.
        /// </summary>
        /// <returns>
        ///     The calculated IP checksum.
        /// </returns>
        public ushort CalculateIPChecksum()
        {
            //copy the ip header
            var theHeader = this.Header;
            var ip = new byte[theHeader.Length];
            Array.Copy(theHeader, ip, theHeader.Length);

            //reset the checksum field (checksum is calculated when this field is zeroed)
            var theValue = (UInt16) 0;
            EndianBitConverter.Big.CopyBytes(theValue, ip, IPv4Fields.ChecksumPosition);

            //calculate the one's complement sum of the ip header
            var cs = ChecksumUtils.OnesComplementSum(ip, 0, ip.Length);

            return (ushort) cs;
        }

        /// <summary>
        ///     Update the checksum value
        /// </summary>
        public void UpdateIPChecksum()
        {
            this.Checksum = this.CalculateIPChecksum();
        }

        /// <summary>
        ///     Prepend to the given byte[] origHeader the portion of the IPv6 header used for
        ///     generating an tcp checksum
        ///     http://en.wikipedia.org/wiki/Transmission_Control_Protocol#TCP_checksum_using_IPv4
        ///     http://tools.ietf.org/html/rfc793
        /// </summary>
        /// <param name="origHeader">
        ///     A <see cref="System.Byte" />
        /// </param>
        /// <returns>
        ///     A <see cref="System.Byte" />
        /// </returns>
        internal override byte[] AttachPseudoIPHeader(byte[] origHeader)
        {
            log.DebugFormat("origHeader.Length {0}", origHeader.Length);

            var odd = origHeader.Length%2 != 0;
            var numberOfBytesFromIPHeaderUsedToGenerateChecksum = 12;
            var headerSize = numberOfBytesFromIPHeaderUsedToGenerateChecksum + origHeader.Length;
            if(odd) { headerSize++; }

            var headerForChecksum = new byte[headerSize];
            // 0-7: ip src+dest addr
            Array.Copy(this.header.Bytes, this.header.Offset + IPv4Fields.SourcePosition, headerForChecksum, 0, IPv4Fields.AddressLength*2);
            // 8: always zero
            headerForChecksum[8] = 0;
            // 9: ip protocol
            headerForChecksum[9] = (byte) this.Protocol;
            // 10-11: header+data length
            var length = (Int16) origHeader.Length;
            EndianBitConverter.Big.CopyBytes(length, headerForChecksum, 10);

            // prefix the pseudoHeader to the header+data
            Array.Copy(origHeader, 0, headerForChecksum, numberOfBytesFromIPHeaderUsedToGenerateChecksum, origHeader.Length);

            //if not even length, pad with a zero
            if(odd) { headerForChecksum[headerForChecksum.Length - 1] = 0; }

            return headerForChecksum;
        }

        /// <summary>
        ///     Construct an instance by values
        /// </summary>
        public IPv4Packet(IPAddress SourceAddress, IPAddress DestinationAddress)
        {
            // allocate memory for this packet
            var offset = 0;
            var length = IPv4Fields.HeaderLength;
            var headerBytes = new byte[length];
            this.header = new ByteArraySegment(headerBytes, offset, length);

            // set some default values to make this packet valid
            this.PayloadLength = 0;
            this.HeaderLength = (HeaderMinimumLength/4); // NOTE: HeaderLength is the number of 32bit words in the header
            this.TimeToLive = this.DefaultTimeToLive;

            // set instance values
            this.SourceAddress = SourceAddress;
            this.DestinationAddress = DestinationAddress;
            this.Version = ipVersion;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="bas">
        ///     A <see cref="ByteArraySegment" />
        /// </param>
        public IPv4Packet(ByteArraySegment bas)
        {
            log.Debug("");

            this.header = new ByteArraySegment(bas);

            // TOS? See http://en.wikipedia.org/wiki/TCP_offload_engine
            if(this.TotalLength == 0) { this.TotalLength = this.header.Length; }

            // Check that the TotalLength is valid, at least HeaderMinimumLength long
            if(this.TotalLength < HeaderMinimumLength) { throw new InvalidOperationException("TotalLength " + this.TotalLength + " < HeaderMinimumLength " + HeaderMinimumLength); }

            // update the header length with the correct value
            // NOTE: we take care to convert from 32bit words into bytes
            // NOTE: we do this *after* setting header because we need header to be valid
            //       before we can retrieve the HeaderLength property
            this.header.Length = this.HeaderLength*4;

            log.DebugFormat("IPv4Packet HeaderLength {0}", this.HeaderLength);
            log.DebugFormat("header {0}", this.header);

            // parse the payload
            if(this.FragmentOffset == 0) //If fragmentOffset is not 0, then packet cannot be further parsed Edit:Pluskal
            {
                var payload = this.header.EncapsulatedBytes(this.PayloadLength);
                this.payloadPacketOrData = ParseEncapsulatedBytes(payload, this.NextHeader, this);
            }
        }

        /// <summary cref="Packet.ToString(StringOutputType)" />
        public override string ToString(StringOutputType outputFormat)
        {
            var buffer = new StringBuilder();
            var color = "";
            var colorEscape = "";

            if(outputFormat == StringOutputType.Colored || outputFormat == StringOutputType.VerboseColored)
            {
                color = this.Color;
                colorEscape = AnsiEscapeSequences.Reset;
            }

            if(outputFormat == StringOutputType.Normal || outputFormat == StringOutputType.Colored)
            {
                // build the output string
                buffer.AppendFormat("{0}[IPv4Packet: SourceAddress={2}, DestinationAddress={3}, HeaderLength={4}, Protocol={5}, TimeToLive={6}]{1}", color, colorEscape,
                    this.SourceAddress, this.DestinationAddress, this.HeaderLength, this.Protocol, this.TimeToLive);
            }

            if(outputFormat == StringOutputType.Verbose || outputFormat == StringOutputType.VerboseColored)
            {
                // collect the properties and their value
                var properties = new Dictionary<string, string>();
                properties.Add("version", this.Version.ToString());
                // FIXME: Header length output is incorrect
                properties.Add("header length", this.HeaderLength + " bytes");
                var diffServices = Convert.ToString(this.DifferentiatedServices, 2).PadLeft(8, '0').Insert(4, " ");
                properties.Add("differentiated services", "0x" + this.DifferentiatedServices.ToString("x").PadLeft(2, '0'));
                properties.Add("", diffServices.Substring(0, 7) + ".. = [" + (this.DifferentiatedServices >> 2) + "] code point");
                properties.Add(" ", ".... .." + diffServices[6] + ". = [" + diffServices[6] + "] ECN");
                properties.Add("  ", ".... ..." + diffServices[7] + " = [" + diffServices[7] + "] ECE");
                properties.Add("total length", this.TotalLength.ToString());
                properties.Add("identification", "0x" + this.Id.ToString("x") + " (" + this.Id + ")");
                var flags = Convert.ToString(this.FragmentFlags, 2).PadLeft(8, '0').Substring(5, 3);
                properties.Add("flags", "0x" + this.FragmentFlags.ToString("x").PadLeft(2, '0'));
                properties.Add("   ", flags[0] + ".. = [" + flags[0] + "] reserved");
                properties.Add("    ", "." + flags[1] + ". = [" + flags[1] + "] don't fragment");
                properties.Add("     ", ".." + flags[2] + " = [" + flags[2] + "] more fragments");
                properties.Add("fragment offset", this.FragmentOffset.ToString());
                properties.Add("time to live", this.TimeToLive.ToString());
                properties.Add("protocol", this.Protocol + " (0x" + this.Protocol.ToString("x") + ")");
                properties.Add("header checksum", "0x" + this.Checksum.ToString("x") + " [" + (this.ValidChecksum? "valid" : "invalid") + "]");
                properties.Add("source", this.SourceAddress.ToString());
                properties.Add("destination", this.DestinationAddress.ToString());

                // calculate the padding needed to right-justify the property names
                var padLength = RandomUtils.LongestStringLength(new List<string>(properties.Keys));

                // build the output string
                buffer.AppendLine("IP:  ******* IPv4 - \"Internet Protocol (Version 4)\" - offset=? length=" + this.TotalPacketLength);
                buffer.AppendLine("IP:");
                foreach(var property in properties)
                {
                    if(property.Key.Trim() != "") {
                        buffer.AppendLine("IP: " + property.Key.PadLeft(padLength) + " = " + property.Value);
                    }
                    else
                    {
                        buffer.AppendLine("IP: " + property.Key.PadLeft(padLength) + "   " + property.Value);
                    }
                }
                buffer.AppendLine("IP:");
            }

            // append the base class output
            buffer.Append(base.ToString(outputFormat));

            return buffer.ToString();
        }

        /// <summary>
        ///     Generate a random packet
        /// </summary>
        /// <returns>
        ///     A <see cref="Packet" />
        /// </returns>
        public static IPv4Packet RandomPacket()
        {
            var srcAddress = RandomUtils.GetIPAddress(ipVersion);
            var dstAddress = RandomUtils.GetIPAddress(ipVersion);
            return new IPv4Packet(srcAddress, dstAddress);
        }

        /// <summary>
        ///     Update the length fields
        /// </summary>
        public override void UpdateCalculatedValues()
        {
            // update the length field based on the length of this packet header
            // plus the length of all of the packets it contains
            this.TotalLength = this.TotalPacketLength;
        }
    }
}