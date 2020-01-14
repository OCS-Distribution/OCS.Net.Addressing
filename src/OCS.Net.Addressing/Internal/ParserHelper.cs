using System;
using System.Globalization;

namespace OCS.Net.Addressing.Internal
{
    internal static class ParserHelper
    {
        internal static bool TryParseIPv4(
            ReadOnlySpan<char> ip,
            
            int startPosition, // start parse from this position 
            out int endPosition, // position of end parsing
            
            out IPv4AddressInternal result
        )
        {
            result = new IPv4AddressInternal();
            
            var segmentIdx = 0;
            
            // current character index and end position initializing
            var i = endPosition = startPosition;
            
            do
            {
                if (!ParseSegment(ref ip, ref i, ref segmentIdx, ref result)) return false;
                
                if (i == ip.Length) break;

                var ch = ip[i];
                if (ch == FormatAndStructureInfo.IPv4SegmentDelimiter) continue; 
                else if (ch == FormatAndStructureInfo.NetworkMaskDelimiter) break;
                else return false;
                
            } while (i++ < ip.Length);

            endPosition = i;
            
            // all segments was parsed validation
            return segmentIdx == FormatAndStructureInfo.IPv4SegmentsCount;
        }

        private static bool ParseSegment(ref ReadOnlySpan<char> ip, 
                                         ref int currentIdx, 
                                         ref int segmentIdx, 
                                         ref IPv4AddressInternal result)
        {
            var atLeastOne = false;
            // use variable for storing current segment value
            // for avoiding indexer multiple calls result[segmentIdx]
            var segment = 0;
            
            for (; currentIdx < ip.Length; currentIdx++)
            {
                // store current character to the local variable ch
                // for avoiding indexer multiple calls ip[i]
                var ch = ip[currentIdx];
                if ('0' <= ch && ch <= '9')
                {
                    // convert character code to digit
                    // decimal digits are placed sequentially in the encoding table 
                    // so if subtract character code of '0' result will equal needed digit
                    // example: '0' = 230, '9' = 239 => '7' - '0' = 237 - 230 = 7
                    var val = ch - '0';
            
                    // because of iterate from high to low digits
                    // need multiplication on 10 (number base) as correction
                    // example: ip = 192. ..
                    // iteration 0: segment = 0,  val = 1 => segment = 1
                    // iteration 1: segment = 1,  val = 9 => segment = 19
                    // iteration 2: segment = 19, val = 2 => segment = 191
                    segment = segment * 10 + val;
                    atLeastOne = true;
                }
                else
                {
                    break;
                }
            }

            // disallow empty segments like 192.168..1
            if (!atLeastOne) return false;
                    
            // fail if current segment value is more than max allowed segment value
            if (segment > FormatAndStructureInfo.IPv4MaxSegmentValue)
                return false;
            
            // stop if address contains too many segments
            if (segmentIdx >= FormatAndStructureInfo.IPv4SegmentsCount)
                return false;
                    
            // store current parsed segment to result variable
            // increase forward current segment index
            // clear segment
            result[segmentIdx++] = (byte) segment;

            return true;
        }

        internal static bool TryParseCDR(ReadOnlySpan<char> cdr, out byte result)
        {
            return byte.TryParse(cdr, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
        }
    }
}