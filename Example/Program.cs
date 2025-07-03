using System.Diagnostics;
using BasicMathExtension.DefaultImplementations;
using CommonLoggers;
using GenericBytecode;

var push = InstructionManager.GetNextInstruction("Push");
var callMethod = InstructionManager.GetNextInstruction("CallMethod");
var ret = InstructionValue.Ret;

var mainBody = new FunctionBytecode([
    new Instruction(push, new InstructionAction(PushSmth)),
    new Instruction(push, new InstructionAction(PushSmth2)),
    new Instruction(BasicMathExtension.BasicMathExtension.DivInstruction, []),
    new Instruction(push, new InstructionAction(PushSmth2)),
    new Instruction(BasicMathExtension.BasicMathExtension.SubInstruction, []),
    new Instruction(callMethod, new InstructionAction(Print)),
    new Instruction(ret, []),
]);

var main = new GenericBytecodeFunction("Main", mainBody);
var module = new GenericBytecodeModule([main]);

var ext = new BasicMathExtension.BasicMathExtension();
module = ext.ManipulateBytecode(module);

var vm = new GenericBytecodeVirtualMachine.GenericBytecodeVirtualMachine();
vm.Init(new GenericBytecodeConfiguration(module, new PlugLogger()));

var sw = Stopwatch.StartNew();
vm.RunModule();
Console.WriteLine(sw.Elapsed);

return;

static void Print(IBasicValue value) => Console.WriteLine(value);
static void PushSmth(out Number res) => res = new Number(10);
static void PushSmth2(out Number res) => res = new Number(25);