public class SolveOrder : Order
{
	public string[] Pattern { get; }

	public SolveOrder(string[] pattern)
	{
		Pattern = pattern;
	}
}
