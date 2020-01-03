using System;
using System.Runtime.CompilerServices;

namespace OCS.Net.Addressing
{
    public partial struct IPv4Address: IEquatable<IPv4Address>, IComparable<IPv4Address>
    {
        #region Equatable
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(IPv4Address other) => this.ipv4.Address == other.ipv4.Address;

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
        public int CompareTo(IPv4Address other) => this.ipv4.Address.CompareTo(other.ipv4.Address);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(IPv4Address lo, IPv4Address ro) => lo.ipv4.Address > ro.ipv4.Address;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(IPv4Address lo, IPv4Address ro) => lo.ipv4.Address < ro.ipv4.Address;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(IPv4Address lo, IPv4Address ro) => lo.ipv4.Address >= ro.ipv4.Address;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(IPv4Address lo, IPv4Address ro) => lo.ipv4.Address <= ro.ipv4.Address;

        #endregion
    }
}