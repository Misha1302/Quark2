namespace DefaultAstImpl.Asg;

public record AsgBuilderConfiguration<T>(SortedDictionary<float, List<INodeCreator<T>>> CreatorLevels);