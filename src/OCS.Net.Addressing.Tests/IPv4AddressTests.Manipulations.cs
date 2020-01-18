using Xunit;

namespace OCS.Net.Addressing.Test
{
    public partial class IPv4AddressTests
    {
        [Fact]
        public void CompareIPv4Address_ShouldCompareCorrect()
        {
            var bigIp = new IPv4Address(10, 10, 10, 255);
            var oneMoreBigIp = new IPv4Address(10, 10, 10, 255);
            var smallIp = new IPv4Address(10, 10, 10, 1);
            
            Assert.True(smallIp != bigIp);
            Assert.True(bigIp == oneMoreBigIp);
        }

        [Theory]
        [InlineData("192.172.40.24")]
        [InlineData("10.10.0.1")]
        public void ParseValidIPv4AndCallToString_ShouldReturnOriginalString(string ip)
        {
            var parsed = IPv4Address.Parse(ip);
            
            Assert.Equal(ip, parsed.ToString());
        }
    }
}