using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes;

public class StringNodeCreator() : ConstantNodeCreatorBase(QuarkLexemeType.Number, AsgNodeType.Number);