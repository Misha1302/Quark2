﻿using System.Reflection;
using System.Reflection.Emit;
using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
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
            new ToMsilTranslatorRuntimeData(constants, methods.ToDictionary(x => x.Name, x => x), new Stack<Any>());
        var result = RuntimeLibrary.CallFunc("Main");
        return [result];
    }

    private (List<DynamicMethod>, List<Any>) CompileModule(BytecodeModule module)
    {
        var constants = new List<Any>();
        var methods = module.Functions.Select(function => CompileFunction(module, function, constants)).ToList();
        return (methods, constants);
    }

    private DynamicMethod CompileFunction(BytecodeModule module, BytecodeFunction function, List<Any> constants)
    {
        var dynamicMethod = new DynamicMethod(
            function.Name,
            typeof(Any),
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

        Console.WriteLine(il.GetILCode());
        return dynamicMethod;
    }


    private void CompileInstruction(GroboIL il, BytecodeInstruction instruction, FunctionCompileData data,
        BytecodeModule module, List<Any> constants)
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
            il.Call(GetInfo(instruction.Arguments[0].Get<Delegate>()));
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

    private void PushConst(GroboIL il, BytecodeInstruction instruction, List<Any> constants)
    {
        constants.Add(instruction.Arguments[0]);
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