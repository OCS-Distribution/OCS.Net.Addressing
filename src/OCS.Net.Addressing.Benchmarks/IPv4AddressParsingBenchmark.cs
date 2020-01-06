using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using CommandLine;

namespace OCS.Net.Addressing.Benchmark
{
    [MemoryDiagnoser]
    public class IPv4AddressParsingBenchmark
    {
        [Params("10.10.0.8", "192.168.1.1", "255.255.255.255", "0.0.0.0")]
        public string IPForParsing { get; set; }

        [Benchmark]
        public void ParseCurrent()
        {
            _ = IPv4Address.Parse(IPForParsing);
        }

        [Benchmark(Baseline = true)]
        public void ParseStdLib()
        {
            _ = IPAddress.Parse(IPForParsing);
        }
    }
}