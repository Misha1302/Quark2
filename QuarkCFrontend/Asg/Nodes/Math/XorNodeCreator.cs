using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class XorNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Xor, LexemeType.Xor);