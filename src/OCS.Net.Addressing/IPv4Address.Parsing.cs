using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address
    {
        private const char IPv4SegmentDelimiter = '.';
        
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
            if (String.IsNullOrWhiteSpace(ip))
                ThrowArgumentException();

            var result = new IPv4AddressInternal();
            var segmentIdx = 0;
            // use variable for storing current segment value
            // for avoiding indexer multiple calls result[segmentIdx]
            byte segment = 0;

            for (var i = 0; i < ip.Length; i++)
            {
                // store current character to the local variable
                // for avoiding indexer multiple calls ip[i]
                var ch = ip[i];
                
                // if current character is segment delimiter
                if (ch == IPv4SegmentDelimiter)
                {
                    // store current parsed segment to result variable
                    // increase forward current segment index
                    // clear segment
                    result[segmentIdx++] = segment;
                    segment = 0;
                }
                // !!! NOTE: noncanonical ip like 0300.0300.0300.0300 or 0xff.0xff.0xff.0xff isn't supported
                // if current character is a decimal digit
                else if ('0' <= ch && ch <= '9')
                {
                    // convert character code to digit
                    // decimal digits are placed sequentially in the encoding table 
                    // so if subtract character code of '0' result will equal needed digit
                    // example: '0' = 230, '9' = 239 => '7' - '0' = 237 - 230 = 7
                    var val = ch - '0';
                    checked
                    {
                        // because of iterate from high to low digits
                        // need multiplication on 10 (number base) as correction
                        // example: ip = 192. ..
                        // iteration 0: segment = 0,  val = 1 => segment = 1
                        // iteration 1: segment = 1,  val = 9 => segment = 19
                        // iteration 2: segment = 19, val = 2 => segment = 191
                        segment = (byte) (segment * 10 + val);
                    }
                }
                else ThrowArgumentException();
            }

            // a valid ip consists of four segments so currentIdx should be equal 3
            if (segmentIdx !=  IPv4AddressInternal.SegmentsCount - 1)
                ThrowArgumentException();
            
            // save the fourth segment to result variable
            // because a valid ip contains exactly three delimiters
            // and inside parsing loop the delimiter condition is fired only three times
            result[segmentIdx] = segment;
            
            return new IPv4Address(result);
        }

        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ThrowArgumentException() =>
            throw new ArgumentException("Expected valid ip v4 address");
    }
}