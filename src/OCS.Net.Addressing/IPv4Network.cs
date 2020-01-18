using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using OCS.Net.Addressing.Internal;

namespace OCS.Net.Addressing
{
    public struct IPv4Network: IEquatable<IPv4Network>
    {
        public const int MaxCDR = 32;
        public static readonly IPv4Network Empty = new IPv4Network();
        
        private readonly IPv4AddressValue address;
        private readonly byte cdr;

        internal IPv4Network(IPv4AddressValue address, byte cdr)
        {
            this.address = address;
            this.cdr = cdr;
        }

        public IPv4Address NetworkAddress => new IPv4Address(this.address);
        public byte CDR => this.cdr;

        public bool Contains(IPv4Address address)
        {
            // todo: refactore this later
            return this.address.Address == CalculateNetworkAddress(address.InternalAddress, cdr).Address;
        }

        public static IPv4Network Parse(string network)
        {
            if (!TryParse(network, out var result))
                ThrowArgumentException();
            
            return result;
        }

        public static bool TryParse(string network, out IPv4Network result)
        {
            result = Empty;
            
            var spanMask = network.AsSpan();
            if (ParserHelper.TryParseIPv4(spanMask, 0, out var end, out var address) &&
                end != network.Length &&
                ParserHelper.TryParseCDR(spanMask.Slice(end + 1), out var cdr))
            {
                var networkAddress = CalculateNetworkAddress(address, cdr); 
                result = new IPv4Network(networkAddress, cdr);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IPv4AddressValue CalculateNetworkAddress(IPv4AddressValue address, byte cdr)
        {
             return new IPv4AddressValue
             {
                 Address = address.Address & (UInt32.MaxValue << (MaxCDR - cdr))
             };
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentException() =>
            throw new ArgumentException("Expected valid ip v4 mask, as example 192.168.0.0/16");

        public bool Equals(IPv4Network other)
        {
            return address.Address == other.address.Address && cdr == other.cdr;
        }

        public override bool Equals(object obj)
        {
            return obj is IPv4Network other && Equals(other);
        }

        public override string ToString()
        {
            return String.Join(
                FormatAndStructureInfo.NetworkMaskDelimiter,
                this.address.ToString(),
                cdr.ToString()
            );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(address, cdr);
        }
    }
}