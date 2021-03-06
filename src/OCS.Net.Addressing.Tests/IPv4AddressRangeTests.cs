using System;
using Xunit;

namespace OCS.Net.Addressing.Test
{
    public class IPv4AddressRangeTests
    {
        [Theory]
        [InlineData(new byte[] {192, 168, 0, 1}, new byte[] {192, 168, 0, 128}, new byte[] {192, 168, 0, 7}, true)]
        [InlineData(new byte[] {10, 10, 0, 200}, new byte[] {10, 10, 10, 200}, new byte[] {10, 10, 5, 200}, true)]
        [InlineData(new byte[] {10, 10, 0, 200}, new byte[] {10, 10, 10, 200}, new byte[] {10, 10, 0, 200}, true)]
        [InlineData(new byte[] {10, 10, 0, 200}, new byte[] {10, 10, 10, 200}, new byte[] {10, 10, 10, 200}, true)]
        [InlineData(new byte[] {192, 168, 0, 1}, new byte[] {192, 168, 0, 128}, new byte[] {192, 168, 128, 7}, false)]
        [InlineData(new byte[] {10, 10, 0, 200}, new byte[] {10, 10, 10, 200}, new byte[] {192, 10, 5, 200}, false)]
        public void Contains_BaseCases(
            byte[] leftBytes, 
            
            byte[] rightBytes, 
            
            byte[] addressBytes,
            
            bool result
        )
        {
            var range = new IPv4AddressRange(
                new IPv4Address(leftBytes),
                new IPv4Address(rightBytes)
            );
            var address = new IPv4Address(addressBytes);
            
            Assert.Equal(result, range.Contains(address));
        }

        [Fact]
        public void Constructor_CreateNewRangeWithIncorrectBorders_ShouldFail()
        {
            var left = new IPv4Address(192, 168, 1, 10);
            var right = new IPv4Address(192, 168, 0, 10);
            
            Assert.Throws<InvalidOperationException>(() => new IPv4AddressRange(left, right));
        }
    }
}