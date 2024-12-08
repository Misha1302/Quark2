using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes;

public class StringNodeCreator() : ConstantNodeCreatorBase(LexemeType.Number, AsgNodeType.Number)
{
}