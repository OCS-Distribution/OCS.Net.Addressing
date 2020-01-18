using System;
using System.Text;

namespace OCS.Net.Addressing
{
    public struct IPv4AddressRange: IEquatable<IPv4AddressRange>
    {
        public static readonly IPv4AddressRange Empty = new IPv4AddressRange();
        
        private readonly IPv4Address left;
        private readonly IPv4Address right;
        
        private readonly bool includeLeft;
        private readonly bool includeRight;

        public IPv4AddressRange(IPv4Address left, 
                                IPv4Address right, 
                                bool includeLeft = false, 
                                bool includeRight = false)
        {
            if (left.InternalAddress.Address > right.InternalAddress.Address)
                throw new InvalidOperationException("Left address must be less than right");
            
            this.left = left;
            this.right = right;
            this.includeLeft = includeLeft;
            this.includeRight = includeRight;
        }

        public IPv4Address Left => left;
        public IPv4Address Right => right;
        public bool IncludeLeft => includeLeft;
        public bool IncludeRight => includeRight;

        public bool Contains(IPv4Address address)
        {
            return (this.includeLeft ? this.left.InternalAddress.Address <= address.InternalAddress.Address 
                                     : this.left.InternalAddress.Address < address.InternalAddress.Address) &&
                   (this.includeRight ? address.InternalAddress.Address <= this.right.InternalAddress.Address  
                                      : address.InternalAddress.Address < this.right.InternalAddress.Address);
        }

        public bool Equals(IPv4AddressRange other)
        {
            return left == other.left && 
                   right == other.right && 
                   includeLeft == other.includeLeft && 
                   includeRight == other.includeRight;
        }

        public override bool Equals(object obj)
        {
            return obj is IPv4AddressRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(left, right, includeLeft, includeRight);
        }

        public override string ToString()
        {
            const int MaxExpectedStringLenght = 33; // max ipv4 lenght is 15 * 2 + 3 char for delimiter and brackets
            
            var builder = new StringBuilder(MaxExpectedStringLenght);

            builder.Append(this.includeLeft ? '[' : '(');
            builder.Append(this.left.ToString());
            builder.Append(':');
            builder.Append(this.right.ToString());
            builder.Append(this.includeRight ? ']' : ')');

            return builder.ToString();
        }
    }
}