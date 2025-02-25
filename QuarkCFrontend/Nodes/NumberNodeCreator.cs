using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes;

public class NumberNodeCreator() : ConstantNodeCreatorBase(QuarkLexemeType.String, AsgNodeType.String);