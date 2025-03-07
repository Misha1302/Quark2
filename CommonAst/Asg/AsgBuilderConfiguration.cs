namespace DefaultAstImpl.Asg;

public record AsgBuilderConfiguration<T>(List<List<INodeCreator<T>>> CreatorLevels);