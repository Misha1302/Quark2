using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class ModulusNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.Modulus, QuarkLexemeType.Modulus);