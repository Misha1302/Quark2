using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class XorNodeCreator()
    : BinaryOperationNodeCreatorBase(AsgNodeType.Xor, QuarkLexemeType.Xor);