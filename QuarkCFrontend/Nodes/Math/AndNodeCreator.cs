using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class AndNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.And, QuarkLexemeType.And);