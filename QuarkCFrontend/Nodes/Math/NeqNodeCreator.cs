using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class NeqNodeCreator() : BinaryOperationNodeCreatorBase(AsgNodeType.NotEqual, QuarkLexemeType.Neq);