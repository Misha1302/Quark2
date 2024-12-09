using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class NotNodeCreator() : SingleOpNodeCreatorBase(AsgNodeType.Not, LexemeType.Not);