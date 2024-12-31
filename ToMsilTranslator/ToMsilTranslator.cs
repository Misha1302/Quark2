using System.Reflection;
using System.Reflection.Emit;
using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using CommonBytecode.Interfaces;
using ExceptionsManager;
using GrEmit;
using static CommonBytecode.Enums.MathLogicOp;

namespace ToMsilTranslator;

public class ToMsilTranslator(ExecutorConfiguration executorConfiguration) : IExecutor
{
    public IEnumerable<Any> RunModule(BytecodeModule module, object?[] arguments)
    {
        var (methods, constants) = CompileModule(module);
        RuntimeLibrary.RuntimeData =
            new ToMsilTranslatorRuntimeData(constants, methods.ToDictionary(x => x.Name, x => x),
                new Stack<TranslatorValue>());
        var result = RuntimeLibrary.CallFunc("Main");
        return [result.ToAny()];
    }

    private (List<DynamicMethod>, List<TranslatorValue>) CompileModule(BytecodeModule module)
    {
        var constants = new List<TranslatorValue>();
        var methods = module.Functions.Select(function => CompileFunction(module, function, constants)).ToList();
        return (methods, constants);
    }

    private DynamicMethod CompileFunction(BytecodeModule module, BytecodeFunction function,
        List<TranslatorValue> constants)
    {
        var dynamicMethod = new DynamicMethod(
            function.Name,
            typeof(TranslatorValue),
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
        BytecodeModule module, List<TranslatorValue> constants)
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
            CallSharp(il, instruction, data);
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
        else if (instruction.Type == InstructionType.PlatformCall)
            Throw.NotImplementedException();
    }

    private void CallSharp(GroboIL il, BytecodeInstruction instruction, FunctionCompileData data)
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

            il.Box(typeof(TranslatorValue));
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
            il.Ldc_I4(i);
            il.Ldelem(typeof(Any));
        }

        il.Call(method);

        if (method.ReturnType != typeof(void))
            il.Call(GetInfo(TranslatorValueExtensions.MakeTranslationValue));

        var q = new TranslatorValue();
        var w = (IAny)q;
        w.GetAnyType();
    }

    private void PushConst(GroboIL il, BytecodeInstruction instruction, List<TranslatorValue> constants)
    {
        constants.Add(instruction.Arguments[0].MakeTranslationValue());
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