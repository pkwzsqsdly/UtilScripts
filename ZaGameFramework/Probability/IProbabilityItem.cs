
public enum ProbabilityType
{
	Item,
	Group
}
public interface IProbabilityItem
{
    ProbabilityType probabilityType { get; }
    int chance { get; }
    int mark { get; }
}