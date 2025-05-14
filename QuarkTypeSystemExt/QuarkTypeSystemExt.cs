using AsgToBytecodeTranslator;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using DictionaryExtensions;
using QuarkExtension;

namespace QuarkTypeSystemExt;

public class QuarkTypeSystemExt : IQuarkExtension
{
    public static readonly QuarkLexemeType Colon = QuarkLexemeTypeHelper.GetNextFreeNumber();

    public LexerConfiguration<QuarkLexemeType> ExtendLexerConfiguration(LexerConfiguration<QuarkLexemeType> current)
    {
        current.Patterns.Add(new LexemePattern<QuarkLexemeType>("\\:", Colon));
        return current;
    }

    public AsgBuilderConfiguration<QuarkLexemeType> ExtendAsgBuilderConfiguration(
        AsgBuilderConfiguration<QuarkLexemeType> current
    )
    {
        current.CreatorLevels.CreateNewOrAdd(1.5f, new IdentifierTypeNodeCreator());
        return current;
    }

    public Action<AsgToBytecodeData<QuarkLexemeType>> GetUnknownAsgCodeHandler() => null!;
}