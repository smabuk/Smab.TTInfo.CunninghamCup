namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class MatchExtensions
{
	extension(Match)
	{
	}

	extension(Match match)
	{
		public Match SetResult(params IEnumerable<Set> sets)
		{
			Result result = new([.. sets], null);
			return match with { Result = result };
		}
		public Match SetResult(params IEnumerable<(int PlayerAScore, int PlayerBScore)> sets)
		{
			Result result = new([.. sets.Select(s => new Set(s.PlayerAScore, s.PlayerBScore))], null);
			return match with { Result = result };
		}
	}
}