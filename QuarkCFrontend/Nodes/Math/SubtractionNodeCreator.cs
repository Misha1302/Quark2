using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class SubtractionNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Subtraction, QuarkLexemeType.Subtraction);