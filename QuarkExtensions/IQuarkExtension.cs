using AsgToBytecodeTranslator;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;

namespace QuarkExtension;

public interface IQuarkExtension
{
    public LexerConfiguration<QuarkLexemeType> ExtendLexerConfiguration(
        LexerConfiguration<QuarkLexemeType> current
    );

    public AsgBuilderConfiguration<QuarkLexemeType> ExtendAsgBuilderConfiguration(
        AsgBuilderConfiguration<QuarkLexemeType> current
    );

    public Action<AsgToBytecodeData<QuarkLexemeType>> GetUnknownAsgCodeHandler();
}