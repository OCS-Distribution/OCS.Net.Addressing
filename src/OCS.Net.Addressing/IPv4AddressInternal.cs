using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OCS.Net.Addressing
{
    [StructLayout(LayoutKind.Explicit, Size = sizeof(UInt32))]
    internal struct IPv4AddressInternal
    {
        [FieldOffset(0)] public UInt32 Address;

        [FieldOffset(0)] public byte Segment1;
        [FieldOffset(1)] public byte Segment2;
        [FieldOffset(2)] public byte Segment3;
        [FieldOffset(3)] public byte Segment4;

        public byte this[int index]
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
            if (index < 0 || index >= SegmentsCount)
                throw new IndexOutOfRangeException();
        }

        public const int SegmentsCount = 4;
    }
}