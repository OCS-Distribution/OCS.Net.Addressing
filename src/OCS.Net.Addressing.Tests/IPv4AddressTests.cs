using System;
using System.Net;
using Xunit;

namespace OCS.Net.Addressing.Test
{
    public partial class IPv4AddressTests
    {
        [Theory]
        [InlineData("192.168.80.11", new byte[] {192, 168, 80, 11}, true)]
        [InlineData("127.0.0.1", new byte[] {127, 0, 0, 1}, true)]
        [InlineData("8.8.8.8", new byte[] {8, 8, 8, 8}, true)]
        [InlineData("10.80.20.11", new byte[] {10, 80, 20, 11}, true)]
        [InlineData("300.168.20.11", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("0xff.11.11.11", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("12.67.32.0300", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("12,67.32.30", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("192.168.1.1.1", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("192.168.1.1.", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("192.168.1", new byte[] {0, 0, 0, 0}, false)]
        [InlineData("192..168.1", new byte[] {0, 0, 0, 0}, false)]
        public void TryParse_BaseCases(string ipStr, byte[] ipBytes, bool correct)
        {
            var expected = new IPv4Address(ipBytes);

            var isParsed = IPv4Address.TryParse(ipStr, out var parsed);

            if (correct)
            {
                Assert.True(isParsed && parsed == expected);
            }
            else
            {
                Assert.False(isParsed);
                Assert.Equal(IPv4Address.Empty, parsed);
            }
        }

        [Fact]
        public void Parse_InvalidIPv4Address_ShouldFail()
        {
            Assert.Throws<ArgumentException>(() => IPv4Address.Parse("0xff.11.11.11"));
        }
        
        [Fact]
        public void Parse_ValidIPv4Address_ShouldParsedSuccessfully()
        {
            var expected = new IPv4Address(10, 11, 11, 11);
            
            var parsed = IPv4Address.Parse("10.11.11.11");
            
            Assert.Equal(expected, parsed);
        }
        
        [Fact]
        public void Equals_BaseCases()
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
        public void ToString_BaseCases(string ip)
        {
            var parsed = IPv4Address.Parse(ip);
            
            Assert.Equal(ip, parsed.ToString());
        }
        
        [Theory]
        [InlineData("192.168.0.1")]
        [InlineData("10.10.10.10")]
        [InlineData("8.8.8.251")]
        public void VerificationAgainstStdLib(string ip)
        {
            var parsedIP = IPv4Address.Parse(ip);
            var stdLibIP = IPAddress.Parse(ip);
            var comparableStdLibIP = new IPv4Address(stdLibIP.GetAddressBytes());
            
            Assert.Equal(comparableStdLibIP, parsedIP);
        }
    }
}