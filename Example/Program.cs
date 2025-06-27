using GenericBytecode2;

var vm = new GenericBytecodeVirtualMachine.GenericBytecodeVirtualMachine();
var mainBody = new FunctionBytecode([
    new Instruction(InstructionManager.GetNextInstruction("Push"), [new InstructionAction(PushSmth)]),
    new Instruction(InstructionManager.GetNextInstruction("Dup"), [new InstructionAction(Dup)]),
    new Instruction(InstructionManager.GetNextInstruction("CallMethod"), [new InstructionAction(Print)]),
    new Instruction(InstructionManager.GetNextInstruction("CallMethod"), [new InstructionAction(Print)]),
    new Instruction(InstructionValue.Ret, []),
]);
var main = new GenericBytecodeFunction("Main", mainBody);
vm.Init(new GenericBytecodeModule([main]));
vm.RunModule();

return;

static void Print(Str value)
{
    Console.WriteLine(value.Value);
}

static void PushSmth(out Str res)
{
    res = new Str("Hi!");
}

static void Dup(Str value, out Str res1, out Str res2)
{
    res1 = value;
    res2 = value;
}

public record Str(string Value) : IBasicValue;