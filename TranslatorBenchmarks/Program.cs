using System.Reflection.Emit;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Benchmarks;
using GrEmit;
using RuntimeHackes;

BenchmarkRunner.Run<TranslatorTest>();

namespace Benchmarks
{
    public class TranslatorTest
    {
        private const int A = 2;
        private const int B = 3;

        private Func<int, int, int> _delegate = null!;
        private DynamicMethod _dynamicMethod = null!;
        private nint _ptr;

        [GlobalSetup]
        public void Setup()
        {
            _dynamicMethod = new DynamicMethod(
                "Sum",
                typeof(int),
                [typeof(int), typeof(int)],
                true
            );

            using var il = new GroboIL(_dynamicMethod);

            il.Ldarg(0);
            il.Ldarg(1);
            il.Add();
            il.Ret();

            _delegate = _dynamicMethod.CreateDelegate<Func<int, int, int>>();
            _ptr = _dynamicMethod.GetNativePointer();
        }


        [Benchmark]
        public int DynamicMethodCall() => (int)_dynamicMethod.Invoke(null, [A, B])!;

        [Benchmark]
        public int DelegateCall() => _delegate(A, B);

        [Benchmark(Baseline = true)]
        public unsafe int PointerCall() => ((delegate*<int, int, int>)_ptr)(A, B);
    }
}