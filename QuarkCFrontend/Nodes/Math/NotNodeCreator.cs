using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkCFrontend.Nodes.Math;

public class NotNodeCreator() : SingleOpNodeCreatorBase(AsgNodeType.Not, QuarkLexemeType.Not);