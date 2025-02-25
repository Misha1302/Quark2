using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace QuarkCFrontend.Nodes.Math;

public class NotNodeCreator() : SingleOpNodeCreatorBase(AsgNodeType.Not, QuarkLexemeType.Not);