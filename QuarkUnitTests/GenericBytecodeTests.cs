using System.Runtime.CompilerServices;
using BasicMathExtension.DefaultImplementations;
using CommonExtensions;
using CommonLoggers;
using GenericBytecode;
using GenericBytecode.Instruction;
using GenericBytecode.Interfaces;
using WistExtensions;

namespace UnitTests;

public class GenericBytecodeTests
{
    private static Stack<IBasicValue> _valuesToPush = [];
    private readonly InstructionValue _callMethod = InstructionManager.GetNextInstruction("CallMethod");

    private readonly InstructionValue _push = InstructionManager.GetNextInstruction("Push");
    private readonly InstructionValue _ret = InstructionManager.Ret;


    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        DoTest(
            [new Number(123)],
            [
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_ret, []),
            ],
            results => Assert.Multiple(() =>
            {
                Assert.That((List<IBasicValue>) [new Number(123)], Is.EqualTo(results));
            })
        );
    }

    [Test]
    public void Test2()
    {
        DoTest(
            [new Number(12), new Number(43)],
            [
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_ret, []),
            ],
            results => Assert.Multiple(() =>
            {
                Assert.That((List<IBasicValue>) [new Number(43), new Number(12)], Is.EqualTo(results));
            })
        );
    }

    [Test]
    public void Test3()
    {
        DoTest(
            [new Number(12), new Number(43)],
            [
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(BasicMathExtension.BasicMathExtension.AddInstruction, []),
                new Instruction(_ret, []),
            ],
            results => Assert.Multiple(() =>
            {
                Assert.That((List<IBasicValue>) [new Number(12 + 43)], Is.EqualTo(results));
            }),
            new BasicMathExtension.BasicMathExtension()
        );
    }

    [Test]
    public void Test4()
    {
        DoTest(
            [new Number(12), new Number(43)],
            [
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(BasicMathExtension.BasicMathExtension.DivInstruction, []),
                new Instruction(_ret, []),
            ],
            results => Assert.Multiple(() =>
            {
                Assert.That((List<IBasicValue>) [new Number(12.0 / 43.0)], Is.EqualTo(results));
            }),
            new BasicMathExtension.BasicMathExtension()
        );
    }

    [Test]
    [MethodImpl(MethodImplOptions.NoOptimization)]
    public void Test5()
    {
        // check for equality of 6,299999999999997 and 6,300000000000004
        // 42.3 % 12 == (42 + 0.1 + 0.2) % 12.0
        // so that the compiler does not precalculate values to turn off the optimization via MethodImpl

        // ReSharper disable once ConvertToConstant.Local
        var value = 42 + 0.1;
        DoTest(
            [new Number(42.3), new Number(12)],
            [
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(_push, new InstructionAction(PushSmth<Number>)),
                new Instruction(BasicMathExtension.BasicMathExtension.ModInstruction, []),
                new Instruction(_ret, []),
            ],
            results => Assert.Multiple(() =>
            {
                Assert.That((List<IBasicValue>) [new Number((value + 0.2) % 12.0)], Is.EqualTo(results));
            }),
            new BasicMathExtension.BasicMathExtension()
        );
    }

    private void DoTest(IEnumerable<IBasicValue> stack, Action action)
    {
        try
        {
            _valuesToPush = new Stack<IBasicValue>(stack.Reverse());
            action();
        }
        finally
        {
            _valuesToPush = [];
        }
    }

    private void DoTest(
        IEnumerable<IBasicValue> stack,
        List<Instruction> instructions,
        Action<IBasicValue[]> check,
        params IWistBytecodeExtension[] exts
    )
    {
        var mainBody = new FunctionBytecode(instructions);
        var main = new GenericBytecodeFunction("Main", mainBody);
        var module = new GenericBytecodeModule([main]);

        module = exts.Aggregate(
            module,
            (current, ext) => ext.ManipulateBytecode(current)
        );

        var vm = new GenericBytecodeVirtualMachine.GenericBytecodeVirtualMachine();
        vm.Init(new GenericBytecodeConfiguration(module, new PlugLogger()));

        DoTest(stack, () => check(vm.RunModule().ToArray()));
    }


    private static void PushSmth<T>(out T res) => res = _valuesToPush.Pop().To<IBasicValue, T>();
}