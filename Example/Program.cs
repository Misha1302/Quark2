using System.Diagnostics;
using CommonLoggers;
using GenericBytecode2;
using GenericBytecode2.Structures;
using Boolean = GenericBytecode2.Structures.Boolean;

var push = InstructionManager.GetNextInstruction("Push");
var callMethod = InstructionManager.GetNextInstruction("CallMethod");
var ret = InstructionValue.Ret;
var setLabel = InstructionValue.SetLabel;
var jumpIfTrue = InstructionValue.JumpIfTrue;

var mainBody = new FunctionBytecode([
    new Instruction(setLabel, [new InstructionAction(GetStartLabelName)]),
    new Instruction(push, [new InstructionAction(PushSmth)]),
    new Instruction(callMethod, [new InstructionAction(Print)]),
    new Instruction(push, [new InstructionAction(PushNeedToContinue)]),
    new Instruction(jumpIfTrue, [new InstructionAction(GetStartLabelName)]),
    new Instruction(ret, []),
]);
var main = new GenericBytecodeFunction("Main", mainBody);

var vm = new GenericBytecodeVirtualMachine.GenericBytecodeVirtualMachine();
vm.Init(new GenericBytecodeConfiguration(new GenericBytecodeModule([main]), new PlugLogger()));

var sw = Stopwatch.StartNew();
vm.RunModule();
Console.WriteLine(sw.Elapsed);

return;

static void GetStartLabelName(out Str res) => res = Temp.Start;
static void Print(Str value) => Console.WriteLine(value.Value);
static void PushSmth(out Str res) => res = new Str("Hi!");
static void PushNeedToContinue(out IBoolean res) => res = new Boolean(++Temp.Count < 100_000);

public static class Temp
{
    public static int Count;
    public static readonly Str Start = new("start");
}