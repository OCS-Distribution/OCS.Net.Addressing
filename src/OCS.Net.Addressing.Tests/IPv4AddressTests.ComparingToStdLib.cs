using System.Net;
using Xunit;

namespace OCS.Net.Addressing.Test
{
    public partial class IPv4AddressTests
    {
        [Theory]
        [InlineData("192.168.0.1")]
        [InlineData("10.10.10.10")]
        [InlineData("8.8.8.251")]
        public void ParseValidIPv4Address_ResultShouldBeEqualSystemNetIPAddressParse(string ip)
        {
            var parsedIP = IPv4Address.Parse(ip);
            var stdLibIP = IPAddress.Parse(ip);
            var comparableStdLibIP = new IPv4Address(stdLibIP.GetAddressBytes());
            
            Assert.Equal(comparableStdLibIP, parsedIP);
        }
    }
}