using CommonFrontendApi;
using DefaultAstImpl.Asg;

namespace WistExtensions;

public interface IWistSyntaxExtension
{
    public LexerConfiguration<T> ExtendLexerConfiguration<T>(LexerConfiguration<T> configuration);

    public AsgBuilderConfiguration<T> ExtendParserConfiguration<T>(AsgBuilderConfiguration<T> configuration);
}