using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class AdditionNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Addition, QuarkLexemeType.Addition);