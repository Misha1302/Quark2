using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class PowerNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Power, QuarkLexemeType.Power);