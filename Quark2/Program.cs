using System.Diagnostics;
using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using SharpLibrariesImporter;
using VirtualMachine;
using static BytecodeGenerationSimplifier.SimpleBytecodeGenerator;
using Any = CommonBytecode.Data.AnyValue.Any;

var importsManager = new ImportsManager();

importsManager.Import("../../../../Libraries");

GetForLoopBlocks(out var mainForStartI, out var mainForCondI, out var mainForStepI, "i");
GetForLoopBlocks(out var mainForStartJ, out var mainForCondJ, out var mainForStepJ, "j");


/*
import 'Libraries'

def main() {
   var len = 100

   var arr = CreateVector()
   SetSize(arr)

   for var i = 0, i < len, i = i + 1 {
       SetValue(arr, i, Random.RandInteger(-10, 10)
   }

   PrintLn(arr)
   BubbleSort(arr)
   PrintLn(arr)
}

def BubbleSort(var arr) {
    var len = GetSize(arr) - 1
    for var i = 0, i < len, i = i + 1 {
        for var j = 0, j < len, j = j + 1 {
            if GetValue(arr, j) > GetValue(arr, j + 1) {
               SwapValues(arr, j, j + 1)
            }
        }
    }
}
*/

var mainForBody = (Func<List<BytecodeInstruction>>)(
    () =>
    [
        // arr[i] = Random.RandInteger(-10, 10)
        new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
        new BytecodeInstruction(InstructionType.LoadLocal, ["i"]),

        new BytecodeInstruction(InstructionType.PushConst, [-10.0]),
        new BytecodeInstruction(InstructionType.PushConst, [10.0]),
        ..CallSharp(importsManager.GetDelegateByName("GetRandomInteger")),

        ..CallSharp(importsManager.GetDelegateByName("SetValue")),
    ]);

var main = (List<BytecodeInstruction>)
[
    // var len = 10
    ..DefineLocals(("len", Number)),
    new BytecodeInstruction(InstructionType.PushConst, [100.0]),
    new BytecodeInstruction(InstructionType.SetLocal, ["len"]),

    // var arr = Vector.CreateVector()
    ..DefineLocals(("arr", SomeSharpObject)),
    ..CallSharp(importsManager.GetDelegateByName("CreateVector")),
    new BytecodeInstruction(InstructionType.SetLocal, ["arr"]),

    // arr.SetSize(len)
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["len"]),
    ..CallSharp(importsManager.GetDelegateByName("SetSize")),

    // for 0..len via i { arr[i] = RandomInteger(-10, 10) }
    ..For(mainForStartI, mainForCondI, mainForStepI, mainForBody),

    // PrintLn(arr)
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    ..CallSharp(importsManager.GetDelegateByName("PrintLn")),

    // BubbleSort(arr, 0)
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.PushConst, [0.0]),
    new BytecodeInstruction(InstructionType.CallFunc, ["BubbleSort"]),

    // PrintLn(arr)
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    ..CallSharp(importsManager.GetDelegateByName("PrintLn")),

    new BytecodeInstruction(InstructionType.PushConst, [Any.Nil]),
    new BytecodeInstruction(InstructionType.Ret, []),
];


var bubbleForBody2 = (Func<List<BytecodeInstruction>>)(() =>
[
    // if arr[j] > arr[j+1]
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["j"]),
    ..CallSharp(importsManager.GetDelegateByName("GetValue")),

    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["j"]),
    new BytecodeInstruction(InstructionType.PushConst, [1.0]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Sum.ToAny()]),
    ..CallSharp(importsManager.GetDelegateByName("GetValue")),

    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Gt.ToAny()]),
    new BytecodeInstruction(InstructionType.BrOp, [BranchMode.IfFalse.ToAny(), "end"]),


    // SwapValues(arr, j, j+1)
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["j"]),

    new BytecodeInstruction(InstructionType.LoadLocal, ["j"]),
    new BytecodeInstruction(InstructionType.PushConst, [1.0]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Sum.ToAny()]),

    ..CallSharp(importsManager.GetDelegateByName("SwapValues")),


    new BytecodeInstruction(InstructionType.Label, ["end"]),
]);


var bubbleForBody = (Func<List<BytecodeInstruction>>)(() =>
[
    ..For(mainForStartJ, mainForCondJ, mainForStepJ, bubbleForBody2),
]);

var recursiveBubbleSort = (List<BytecodeInstruction>)
[
    // read parameters - arr and i
    ..DefineLocals(("arr", SomeSharpObject), ("len", Number), ("i", Number)),
    ..ReadParameters("arr", "i"),

    // len = GetSize(arr) - 1
    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    ..CallSharp(importsManager.GetDelegateByName("GetSize")),
    new BytecodeInstruction(InstructionType.PushConst, [1.0]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Sub.ToAny()]),
    new BytecodeInstruction(InstructionType.SetLocal, ["len"]),

    // if i == len { return }
    new BytecodeInstruction(InstructionType.LoadLocal, ["i"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["len"]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Lt.ToAny()]),
    new BytecodeInstruction(InstructionType.BrOp, [BranchMode.IfTrue.ToAny(), "sort"]),

    new BytecodeInstruction(InstructionType.Ret, []),

    new BytecodeInstruction(InstructionType.Label, ["sort"]),


    // for 0..len via i { arr[i] = RandomInteger(-10, 10) }
    ..For(mainForStartJ, mainForCondJ, mainForStepJ, bubbleForBody),

    new BytecodeInstruction(InstructionType.LoadLocal, ["arr"]),
    new BytecodeInstruction(InstructionType.LoadLocal, ["i"]),
    new BytecodeInstruction(InstructionType.PushConst, [1.0]),
    new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Sum.ToAny()]),
    new BytecodeInstruction(InstructionType.CallFunc, ["BubbleSort"]),

    new BytecodeInstruction(InstructionType.PushConst, [Any.Nil]),
    new BytecodeInstruction(InstructionType.Ret, []),
];

var module = new BytecodeModule(
    "print cubes",
    [
        new BytecodeFunction("Main", new Bytecode(main)),
        new BytecodeFunction("BubbleSort", new Bytecode(recursiveBubbleSort)),
    ]
);

var executor = (IExecutor)new QuarkVirtualMachine();
var sw = Stopwatch.StartNew();
var results = executor.RunModule(module, [null]);
Console.WriteLine(sw.ElapsedMilliseconds);
Console.WriteLine(string.Join(", ", results));
return;

void GetForLoopBlocks(
    out Func<List<BytecodeInstruction>> mainForStartParam,
    out Func<List<BytecodeInstruction>> mainForCondParam,
    out Func<List<BytecodeInstruction>> mainForStepParam,
    string varName
)
{
    mainForStartParam = () =>
    [
        // var i = 0
        ..DefineLocals((varName, Number)),
        new BytecodeInstruction(InstructionType.PushConst, [0.0]),
        new BytecodeInstruction(InstructionType.SetLocal, [varName]),
    ];

    mainForCondParam = () =>
    [
        // i < len
        new BytecodeInstruction(InstructionType.LoadLocal, [varName]),
        new BytecodeInstruction(InstructionType.LoadLocal, ["len"]),
        new BytecodeInstruction(InstructionType.MathOrLogicOp, [MathLogicOp.Lt.ToAny()]),
    ];

    mainForStepParam = () =>
    [
        // i = i + 1
        ..Inc(varName),
    ];
}