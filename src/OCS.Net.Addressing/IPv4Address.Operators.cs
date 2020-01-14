using System;
using System.Runtime.CompilerServices;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address: IEquatable<IPv4Address>, IComparable<IPv4Address>
    {
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
        
        #region Compare
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(IPv4Address other) => this.value.Address.CompareTo(other.value.Address);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(IPv4Address lo, IPv4Address ro) => lo.value.Address > ro.value.Address;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(IPv4Address lo, IPv4Address ro) => lo.value.Address < ro.value.Address;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(IPv4Address lo, IPv4Address ro) => lo.value.Address >= ro.value.Address;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(IPv4Address lo, IPv4Address ro) => lo.value.Address <= ro.value.Address;

        #endregion
    }
}