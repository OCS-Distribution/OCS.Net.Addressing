using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using OCS.Net.Addressing.Internal;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address: IEquatable<IPv4Address>
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
        
        #region Equatable
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(IPv4Address other) => this.value.Address == other.value.Address;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IPv4Address other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(IPv4Address lo, IPv4Address ro) => lo.Equals(ro);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(IPv4Address lo, IPv4Address ro) => !(lo == ro);
        
        #endregion
        
        #region Parsing
        
        public static bool TryParse(string ip, out IPv4Address result)
        {
            if (ParserHelper.TryParseIPv4(
                    ip.AsSpan(), // ip for parsing
                        
                    0, // start parsing position
                    out var endPosition, // position of end parsing, out
                        
                    out var internalResult // result
                ) 
                && endPosition == ip.Length) // all string was parsed check
            {
                result = new IPv4Address(internalResult);
                return true;
            }
            
            result = Empty;
            return false;
        }

        public static IPv4Address Parse(string ip)
        {
            if (!TryParse(ip, out var result))
                ThrowArgumentException();
            
            return result;
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentException() =>
            throw new ArgumentException("Expected valid ip v4 address");
        
        #endregion
    }
}