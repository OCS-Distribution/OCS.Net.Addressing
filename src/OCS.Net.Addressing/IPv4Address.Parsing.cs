using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address
    {
        private const char IPv4SegmentDelimer = '.';
        
        public static bool TryParse(string ip, out IPv4Address result)
        {
            result = IPv4Address.Empty;
            
            try
            {
                result = Parse(ip);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IPv4Address Parse(string ip)
        {
            // use ReadOnlySpan, AsSpan and Parse(ReadOnlySpan ...) for
            // avoid memory allocations in future
            // todo: optimize this with spans after migrations to core 3.1
            
            if (String.IsNullOrWhiteSpace(ip))
                ThrowArgumentException();

            var segments = ip.Split(IPv4SegmentDelimer);
            
            if (segments.Length != IPv4AddressInternal.SegmentsCount)
                ThrowArgumentException();
            
            var result = new IPv4AddressInternal();
            for (var i = 0; i < segments.Length; i++)
                result[i] = byte.Parse(segments[i].AsSpan());

            return new IPv4Address(result);
        }

        // public static IPv4Address Parse2(string ip)
        // {
        //     if (String.IsNullOrWhiteSpace(ip))
        //         ThrowArgumentException();
        //
        //     int segmentIndex = 0;
        //     var result = new IPv4AddressInternal();
        //     for (int i = 0; i < ip.Length; i++)
        //     {
        //         if (ip[i] == IPv4SegmentDelimer)
        //         {
        //         }
        //     }
        // }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentException() =>
            throw new ArgumentException("Expected valid ip v4 address");
    }
}