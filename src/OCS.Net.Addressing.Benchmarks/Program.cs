using System;
using BenchmarkDotNet.Running;

namespace OCS.Net.Addressing.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<IPv4AddressParsingBenchmark>();
        }
    }
}