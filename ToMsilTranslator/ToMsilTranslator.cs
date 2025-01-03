using System.Reflection;
using AbstractExecutor;
using CommonDataStructures;

namespace ToMsilTranslator;

public class ToMsilTranslator : IExecutor
{
    public IEnumerable<Any> RunModule(BytecodeModule module)
    {
        Init(module);
        var result = RuntimeLibrary.CallFunc("Main");
        return [result.ToAny()];
    }

    public IEnumerable<Any> RunFunction(BytecodeModule module, string name, Span<Any> functionArguments)
    {
        Init(module);
        foreach (var argument in functionArguments)
            RuntimeLibrary.RuntimeData.IntermediateData.Push(argument.MakeAnyOpt());
        var result = RuntimeLibrary.CallFunc(name);
        return [result.ToAny()];
    }

    private void Init(BytecodeModule module)
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        if (RuntimeLibrary.RuntimeData?.Module == module) return;
        var (methods, constants) = CompileModule(module);
        RuntimeLibrary.RuntimeData =
            new ToMsilTranslatorRuntimeData(
                constants,
                methods.ToDictionary(x => x.Name, x => x),
                new Stack<AnyOpt>(),
                module
            );
    }

    private (List<DynamicMethod>, List<AnyOpt>) CompileModule(BytecodeModule module)
    {
        var constants = new List<AnyOpt>();
        var methods = module.Functions.Select(function => CompileFunction(module, function, constants)).ToList();
        return (methods, constants);
    }

    private DynamicMethod CompileFunction(BytecodeModule module, BytecodeFunction function,
        List<AnyOpt> constants)
    {
        var dynamicMethod = new DynamicMethod(
            function.Name,
            typeof(AnyOpt),
            [typeof(ToMsilTranslatorRuntimeData)]
        );

        using var il = new GroboIL(dynamicMethod);

        var data = new FunctionCompileData(
            function.GetLocals(il),
            function.GetLabels(il)
        );

        var parametersCount = function.Code.GetParametersCount();
        for (var i = 0; i < parametersCount; i++) il.Call(GetInfo(RuntimeLibrary.PopFromStack));
        // il.Stloc(data.Locals[function.Code.Instructions[parametersCount].Arguments[0].Get<BytecodeVariable>().Name]);
        foreach (var instruction in function.Code.Instructions)
            CompileInstruction(il, instruction, data, module, constants);

        return dynamicMethod;
    }


    private void CompileInstruction(GroboIL il, BytecodeInstruction instruction, FunctionCompileData data,
        BytecodeModule module, List<AnyOpt> constants)
    {
        if (instruction.Type == InstructionType.PushConst)
            PushConst(il, instruction, constants);
        else if (instruction.Type == InstructionType.MathOrLogicOp)
            CompileMathOrLogicOp(il, instruction);
        else if (instruction.Type == InstructionType.LoadLocal)
            il.Ldloc(data.Locals[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.SetLocal)
            il.Stloc(data.Locals[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.BrOp)
            CompileBrOp(il, instruction, data);
        else if (instruction.Type == InstructionType.CallSharp)
            CallSharp(il, instruction);
        else if (instruction.Type == InstructionType.CallFunc)
            CallFunc(il, instruction, module);
        else if (instruction.Type == InstructionType.Ret)
            il.Ret();
        else if (instruction.Type == InstructionType.Label)
            il.MarkLabel(data.Labels[instruction.Arguments[0].Get<string>()]);
        else if (instruction.Type == InstructionType.Drop)
            il.Pop();
        else if (instruction.Type == InstructionType.MakeVariables)
            DoNothing();
    }

    private void CallSharp(GroboIL il, BytecodeInstruction instruction)
    {
        var method = GetInfo(instruction.Arguments[0].Get<Delegate>());
        var parameters = method.GetParameters();

        var arrLoc = il.DeclareLocal(typeof(Any[]));
        il.Ldc_I4(parameters.Length);
        il.Newarr(typeof(Any));
        il.Stloc(arrLoc);
        for (var i = 0; i < parameters.Length; i++)
        {
            var temp = il.DeclareLocal(typeof(Any));

            il.Box(typeof(AnyOpt));
            il.Castclass(typeof(IAny));
            il.Call(GetInfo((Func<IAny, Any>)AnyExtensions.ToAny));
            il.Stloc(temp);

            il.Ldloc(arrLoc);
            il.Ldc_I4(i);
            il.Ldelema(typeof(Any));
            il.Ldloc(temp);

            il.Stobj(typeof(Any));
        }

        for (var i = 0; i < parameters.Length; i++)
        {
            il.Ldloc(arrLoc);
            il.Ldc_I4(parameters.Length - i - 1);
            il.Ldelem(typeof(Any));
        }

        il.Call(method);

        if (method.ReturnType != typeof(void))
            il.Call(GetInfo(AnyOptExtensions.MakeAnyOpt));

        var q = new AnyOpt();
        var w = (IAny)q;
        w.GetAnyType();
    }

    private void PushConst(GroboIL il, BytecodeInstruction instruction, List<AnyOpt> constants)
    {
        constants.Add(instruction.Arguments[0].MakeAnyOpt());
        il.Ldc_I4(constants.Count - 1);
        il.Call(GetInfo(RuntimeLibrary.GetConst));
    }

    private void DoNothing()
    {
    }

    private void CallFunc(GroboIL il, BytecodeInstruction instruction, BytecodeModule module)
    {
        var name = instruction.Arguments[0].Get<string>();
        var func = module.Functions.First(x => x.Name == name);
        var parametersCount = func.Code.GetParametersCount();

        for (var i = 0; i < parametersCount; i++) il.Call(GetInfo(RuntimeLibrary.PushToStack));
        il.Ldstr(name);
        il.Call(GetInfo(RuntimeLibrary.CallFunc));
    }

    private void CompileBrOp(GroboIL il, BytecodeInstruction instruction, FunctionCompileData data)
    {
        var name = instruction.Arguments[1].Get<string>();
        var branch = instruction.Arguments[0].Get<BranchMode>();
        var label = data.Labels[name];

        switch (branch)
        {
            case BranchMode.Basic:
                il.Br(label);
                break;
            case BranchMode.IfTrue:
                il.Call(GetInfo(RuntimeLibrary.ToBool));
                il.Brtrue(label);
                break;
            case BranchMode.IfFalse:
                il.Call(GetInfo(RuntimeLibrary.ToBool));
                il.Brfalse(label);
                break;
            default:
                Throw.InvalidOpEx();
                break;
        }
    }

    private void CompileMathOrLogicOp(GroboIL il, BytecodeInstruction instruction)
    {
        var op = instruction.Arguments[0].Get<MathLogicOp>();

        var c = op switch
        {
            Sum => TranslatorCalc.Sum,
            Sub => TranslatorCalc.Sub,
            Mul => TranslatorCalc.Mul,
            Div => TranslatorCalc.Div,
            Pow => TranslatorCalc.Pow,
            Mod => TranslatorCalc.Mod,
            And => TranslatorCalc.And,
            Or => TranslatorCalc.Or,
            Xor => TranslatorCalc.Xor,
            Eq => TranslatorCalc.Eq,
            NotEq => TranslatorCalc.NotEq,
            Lt => TranslatorCalc.Lt,
            Gt => TranslatorCalc.Gt,
            LtOrEq => TranslatorCalc.LtOrEq,
            GtOrEq => TranslatorCalc.GtOrEq,
            Not => TranslatorCalc.Not,
            _ => Throw.InvalidOpEx<Delegate>(),
        };

        il.Call(GetInfo(c));
    }

    private MethodInfo GetInfo(Delegate del) => del.Method;
}