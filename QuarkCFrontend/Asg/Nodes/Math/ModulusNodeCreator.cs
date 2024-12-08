using QuarkCFrontend.Lexer;

namespace QuarkCFrontend.Asg.Nodes.Math;

public class ModulusNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Modulus, LexemeType.Modulus);