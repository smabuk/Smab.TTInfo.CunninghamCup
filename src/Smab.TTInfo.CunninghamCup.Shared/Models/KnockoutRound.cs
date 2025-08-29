namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record KnockoutRound(RoundType Type, List<Match> Matches)
{
	public override string ToString()
	{
		string typePart = $"{nameof(Type)} = {Type}";
		string matchesPart = $"{nameof(Matches)} = [{string.Join(", ", Matches)}]";
		return $$"""{{nameof(KnockoutRound)}} { {{typePart}}, {{matchesPart}} }""";
	}
}
