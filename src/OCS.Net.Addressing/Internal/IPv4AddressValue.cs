﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace OCS.Net.Addressing.Internal
{
    [StructLayout(LayoutKind.Explicit, Size = sizeof(UInt32))]
    internal struct IPv4AddressValue
    {
        [FieldOffset(0)] internal UInt32 Address;

        [FieldOffset(3)] internal byte Segment1;
        [FieldOffset(2)] internal byte Segment2;
        [FieldOffset(1)] internal byte Segment3;
        [FieldOffset(0)] internal byte Segment4;

        internal byte this[int index]
        {
            get
            {
                AssertIndex(index);

                return index switch
                {
                    0 => Segment1,
                    1 => Segment2,
                    2 => Segment3,
                    _ => Segment4 // idx = 3
                };
            }
            set
            {
                AssertIndex(index);
                
                switch (index)
                {
                    case 0: Segment1 = value; break;
                    case 1: Segment2 = value; break;
                    case 2: Segment3 = value; break;
                    default: Segment4 = value; break; // idx = 3
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AssertIndex(int index)
        {
            if (index < 0 || index >= FormatAndStructureInfo.IPv4SegmentsCount)
                throw new IndexOutOfRangeException();
        }

        public override string ToString()
        {
            const int MaxExcpectedLenght = 15; // 3 digit * 4 segments + 3 dots
            
            var builder = new StringBuilder(MaxExcpectedLenght);

            builder.Append(Segment1.ToString());
            builder.Append(FormatAndStructureInfo.IPv4SegmentDelimiter);
            builder.Append(Segment2.ToString());
            builder.Append(FormatAndStructureInfo.IPv4SegmentDelimiter);
            builder.Append(Segment3.ToString());
            builder.Append(FormatAndStructureInfo.IPv4SegmentDelimiter);
            builder.Append(Segment4.ToString());

            return builder.ToString();
        }
    }
}