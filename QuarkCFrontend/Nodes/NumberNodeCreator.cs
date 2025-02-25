using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes;

public class NumberNodeCreator() : ConstantNodeCreatorBase(QuarkLexemeType.String, AsgNodeType.String);