namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Group(
	string Name,
	List<Player> Players,
	List<Match> Matches
)
{
	public bool IsCompleted => Matches.All(m => m.IsCompleted);

	public List<GroupPlayerSummary> GroupPositions => [.. Players
		.Select(player => new GroupPlayerSummary(
			player,
			Matches.Count(m => (m.IsPlayerAWin && m.PlayerA == player) || (m.IsPlayerBWin && m.PlayerB == player)),
			Matches.Count(m => (m.IsPlayerAWin && m.PlayerB == player) || (m.IsPlayerBWin && m.PlayerA == player)),
			Matches.Where(m => m.PlayerA.Name == player.Name).Sum(m => m.PlayerASets) + Matches.Where(m => m.PlayerB.Name == player.Name).Sum(m => m.PlayerBSets),
			Matches.Where(m => m.PlayerA.Name == player.Name).Sum(m => m.PlayerBSets) + Matches.Where(m => m.PlayerB.Name == player.Name).Sum(m => m.PlayerASets),
			Matches.Where(m => m.PlayerA.Name == player.Name).Sum(m => m.PlayerATotalPoints) + Matches.Where(m => m.PlayerB.Name == player.Name).Sum(m => m.PlayerBTotalPoints),
			Matches.Where(m => m.PlayerA.Name == player.Name).Sum(m => m.PlayerBTotalPoints) + Matches.Where(m => m.PlayerB.Name == player.Name).Sum(m => m.PlayerATotalPoints)
		))
		.OrderByDescending(gp => gp.MatchWins)
		.ThenBy(gp => gp.MatchLosses)
		// TODO: use difference averages as well as totals to break ties
		.ThenByDescending(gp => gp.SetsFor)
		.ThenBy(gp => gp.SetsAgainst)
		.ThenByDescending(gp => gp.PointsFor)
		.ThenBy(gp => gp.PointsAgainst)
		];
}
