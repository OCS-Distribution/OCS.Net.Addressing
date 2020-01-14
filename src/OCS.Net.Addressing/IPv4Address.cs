using System;
using System.Runtime.CompilerServices;
using OCS.Net.Addressing.Internal;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address
    {
        public static readonly IPv4Address Empty = new IPv4Address();
        
        private readonly IPv4AddressValue value;
        
        internal IPv4Address(IPv4AddressValue value)
        {
            this.value = value;
        }

        public IPv4Address(uint address)
            : this(new IPv4AddressValue {Address = address})
        {
            // empty
        }

        public IPv4Address(byte segment1, byte segment2, byte segment3, byte segment4) 
            : this(new IPv4AddressValue()
            {
                Segment1 = segment1,
                Segment2 = segment2,
                Segment3 = segment3,
                Segment4 = segment4,
            })
        {
            // empty
        }
        
        public IPv4Address(byte[] segments)
            :this(BytesToPv4AddressValue(segments))
        {
            // empty
        }

        internal IPv4AddressValue InternalAddress => this.value;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => this.value.Address.GetHashCode();
        
        public override string ToString()
        {
            return String.Join(
                FormatAndStructureInfo.IPv4SegmentDelimiter,
                
                this.value.Segment1.ToString(),
                this.value.Segment2.ToString(),
                this.value.Segment3.ToString(),
                this.value.Segment4.ToString()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IPv4AddressValue BytesToPv4AddressValue(byte[] segments)
        {
            if (segments.Length != FormatAndStructureInfo.IPv4SegmentsCount)
                throw new ArgumentException(
                    message: "IP v4 address should consist of 4 single byte segments exactly",
                    paramName: nameof(segments)
                );

            var address = new IPv4AddressValue();
            for (int i = 0; i < FormatAndStructureInfo.IPv4SegmentsCount; i++)
                address[i] = segments[i];
            
            return address;
        }
    }
}