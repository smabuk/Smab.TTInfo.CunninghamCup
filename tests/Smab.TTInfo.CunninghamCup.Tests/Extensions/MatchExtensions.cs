namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;
public static class MatchExtensions
{
	extension(Match match)
	{
		public Match SetRandomResult()
		{
			bool[] playerAWins = [
				Random.Shared.Next(2) == 0,
				Random.Shared.Next(2) == 0,
				Random.Shared.Next(2) == 0];

			List<Set> sets = playerAWins switch
			{
				[true,  true,  _] => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(21, Random.Shared.Next(match.PlayerBStart, 20))],
				[false, false, _] => [new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21)],
				[_,  _, true] => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(21, Random.Shared.Next(match.PlayerBStart, 20))],
				_ => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21)],
			};

			return match.SetResult(sets);
		}
	}
}
