using DefaultAstImpl.Asg;

namespace WistExtensions;

public interface IWistAstExtension
{
    public AsgNode<T> ManipulateAst<T>(AsgNode<T> root);
}