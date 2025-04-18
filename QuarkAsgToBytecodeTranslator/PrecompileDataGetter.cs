namespace AsgToBytecodeTranslator;

public class PrecompileDataGetter
{
    public List<FunctionData> GetFunctions<T>(AsgNode<T> root)
    {
        var functions = new List<FunctionData>();
        new BytecodeDfs().Dfs(root, x =>
        {
            if (x.NodeType != AsgNodeType.FunctionCreating) return;

            var bytecodeFunction = new BytecodeFunction(x.Text, new Bytecode([]));
            var parameters = x.Children[1].Children.Select(c => new BytecodeVariable(c.Text, AnyValueType.Any));
            var locals = GetLocals(x.Children[2]);
            functions.Add(new FunctionData(bytecodeFunction, parameters.ToList(), locals));
        });
        return functions;
    }

    private List<BytecodeVariable> GetLocals<T>(AsgNode<T> node)
    {
        var locals = new List<BytecodeVariable>();
        new BytecodeDfs().Dfs(node, x =>
        {
            if (x.NodeType != AsgNodeType.SetOperation) return;

            var varName = x.Children[0].Text;
            locals.Add(new BytecodeVariable(varName, AnyValueType.Any));
        });
        return locals;
    }
}