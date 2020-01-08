using System;
using System.Runtime.CompilerServices;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address
    {
        public static readonly IPv4Address Empty = new IPv4Address();
        
        private readonly IPv4AddressInternal ipv4;
        
        internal IPv4Address(IPv4AddressInternal ipv4)
        {
            this.ipv4 = ipv4;
        }

        public IPv4Address(uint address)
            : this(new IPv4AddressInternal {Address = address})
        {
            // empty
        }

        public IPv4Address(byte segment1, byte segment2, byte segment3, byte segment4) 
            : this(new IPv4AddressInternal()
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
            :this(BytesToPv4AddressInternal(segments))
        {
            // empty
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => ipv4.Address.GetHashCode();
        
        public override string ToString()
        {
            return String.Join(
                IPv4SegmentDelimiter.ToString(),
                
                this.ipv4.Segment1.ToString(),
                this.ipv4.Segment2.ToString(),
                this.ipv4.Segment3.ToString(),
                this.ipv4.Segment4.ToString()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IPv4AddressInternal BytesToPv4AddressInternal(byte[] segments)
        {
            if (segments.Length != IPv4AddressInternal.SegmentsCount)
                throw new ArgumentException(
                    message: "IP v4 address should consist of 4 single byte segments exactly",
                    paramName: nameof(segments)
                );

            var address = new IPv4AddressInternal();
            for (int i = 0; i < IPv4AddressInternal.SegmentsCount; i++)
                address[i] = segments[i];
            
            return address;
        }
    }
}