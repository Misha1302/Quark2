using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes;

public class StringNodeCreator() : ConstantNodeCreatorBase(QuarkLexemeType.Number, AsgNodeType.Number);