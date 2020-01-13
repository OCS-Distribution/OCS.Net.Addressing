using Xunit;

namespace OCS.Net.Addressing.Test
{
    public partial class IPv4AddressTests
    {
        [Theory]
        [InlineData(192, 168, 80, 11)]
        [InlineData(127, 0, 0, 1)]
        [InlineData(8, 8, 8, 8)]
        [InlineData(10, 80, 20, 11)]
        public void ParseValidIPv4Address_ShouldReturnCorrectParsedAddress(byte s1, byte s2, byte s3, byte s4)
        {
            var ip = $"{s1}.{s2}.{s3}.{s4}";
            var expected = new IPv4Address(s1, s2, s3, s4);

            var parsed = IPv4Address.Parse(ip);

            Assert.True(
                parsed == expected
            );
        }

        [Theory]
        [InlineData("300.168.20.11")]
        [InlineData("0xff.11.11.11")]
        [InlineData("12.67.32.0300")]
        [InlineData("12,67.32.30")]
        [InlineData("192.168.1.1.1")]
        [InlineData("192.168.1.1.")]
        [InlineData("192.168.1")]
        [InlineData("192..168.1")]
        public void ParseInvalidIPv4Address_ShouldFail(string ip)
        {
            var isParsed = IPv4Address.TryParse(ip, out var result);

            Assert.False(isParsed);
            Assert.Equal(IPv4Address.Empty, result);
        }
    }
}