using BenchmarkDotNet.Running;
using QuarkBenchmarks;

BenchmarkRunner.Run<MillionLoopTest>();
BenchmarkRunner.Run<RecFactorialTest>();
BenchmarkRunner.Run<DfsTest>();
BenchmarkRunner.Run<IsPrimeTest>();