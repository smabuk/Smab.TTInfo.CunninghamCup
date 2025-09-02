namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class MatchIdExtensions
{
	extension(MatchId matchId)
	{
		public int MatchNo
			=> int.TryParse(matchId.StringId.Split(' ').Last(), out int number)
				? number
				: 0;
	}
}
