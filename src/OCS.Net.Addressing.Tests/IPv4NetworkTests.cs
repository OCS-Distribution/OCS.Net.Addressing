using OCS.Net.Addressing.Internal;
using Xunit;

namespace OCS.Net.Addressing.Test
{
    public class IPv4NetworkTests
    {
        [Theory]
        [InlineData("192.168.0.1/24",new byte[] {192, 168, 0, 0}, 24)]
        [InlineData("10.10.10.10/16", new byte[] {10, 10, 0, 0}, 16)]
        [InlineData("254.10.24.12/20", new byte[] {254, 10, 16, 0}, 20)]
        [InlineData("255.255.255.255/20", new byte[] {255, 255, 240, 0}, 20)]
        public void ParseValidIPv4Network_ShouldReturnCorrectParsedNetwork(string network, byte[] addressBytes, byte cdr)
        {
            var networkAddress = new IPv4Address(addressBytes);
            
            var isParsed= IPv4Network.TryParse(network, out var result);

            Assert.True(isParsed);
            Assert.Equal(networkAddress, result.NetworkAddress);
            Assert.Equal(cdr, result.CDR);
        }

        [Theory]
        [InlineData(new byte[] {192, 168, 0, 0}, 24, new byte[] {192, 168, 0, 7})]
        [InlineData(new byte[] {10, 10, 0, 0}, 16, new byte[] {10, 10, 128, 241})]
        [InlineData(new byte[] {254, 10, 16, 0}, 20, new byte[] {254, 10, 16, 255})]
        [InlineData(new byte[] {255, 255, 240, 0}, 20, new byte[] {255, 255, 240, 192})]
        public void ContainsOnAddressesInsideNetwork_ShouldReturnTrue(byte[] networkBytes, byte cdr, byte[] addressBytes)
        {
            var network = new IPv4Network(new IPv4AddressValue
            {
                Segment1 = networkBytes[0],
                Segment2 = networkBytes[1],
                Segment3 = networkBytes[2],
                Segment4 = networkBytes[3],
            }, cdr);
            var address = new IPv4Address(addressBytes);
            
            Assert.True(network.Contains(address));
        }
    }
}