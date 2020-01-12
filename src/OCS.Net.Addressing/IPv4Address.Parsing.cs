using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using OCS.Net.Addressing.Internal;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address
    {   
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
    }
}