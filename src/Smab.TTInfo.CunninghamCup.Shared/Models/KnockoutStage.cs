namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record KnockoutStage(string Name, List<int> GroupPositions, List<KnockoutRound> Rounds)
{
	public override string ToString()
	{
		string namePart = $"{nameof(Name)} = {Name}";
		string groupPositionsPart = $"{nameof(GroupPositions)} = [{string.Join(", ", GroupPositions)}]";
		string roundsPart = $"{nameof(Rounds)} = [{string.Join(", ", Rounds)}]";
		return $$"""{{nameof(KnockoutStage)}} { {{namePart}}, {{groupPositionsPart}}, {{roundsPart}} }""";
	}
}
