namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a group in a tournament, containing a collection of players and matches.
/// </summary>
/// <remarks>A group is a logical unit in a tournament that tracks players, their matches, and the results. It
/// provides functionality to determine whether all matches in the group are completed and to calculate the standings of
/// players based on their performance in the matches.</remarks>
/// <param name="Name">The name of the group.</param>
/// <param name="Players">The list of players participating in the group.</param>
/// <param name="Matches">The list of matches played within the group.</param>
public record Group(
	string Name,
	List<PlayerId> Players,
	List<Match> Matches
)
{
	public bool IsCompleted => Matches.All(m => m.IsCompleted);

	/// <summary>
	/// Gets a list of summaries representing the positions of players in the group,  ranked based on their performance in
	/// matches.
	/// </summary>
	/// <remarks>The ranking logic prioritizes players with the highest number of match wins. In the event of ties, 
	/// additional criteria such as match losses, sets won, sets lost, points scored, and points conceded  are used to
	/// determine the order. Future enhancements may include using difference averages to further  refine
	/// tie-breaking.</remarks>
	public List<GroupPlayerSummary> GroupPositions => [.. Players
		.Select(playerId => new GroupPlayerSummary(
			PlayerId: playerId,
			Played:        Matches.Count(m => m.IsCompleted && ((m.PlayerA == playerId) || (m.PlayerB == playerId))),
			MatchWins:     Matches.Count(m => (m.IsPlayerAWin && m.PlayerA == playerId) || (m.IsPlayerBWin && m.PlayerB == playerId)),
			MatchLosses:   Matches.Count(m => (m.IsPlayerAWin && m.PlayerB == playerId) || (m.IsPlayerBWin && m.PlayerA == playerId)),
			SetsFor:       Matches.Where(m => m.PlayerA == playerId).Sum(m => m.PlayerASets) + Matches.Where(m => m.PlayerB == playerId).Sum(m => m.PlayerBSets),
			SetsAgainst:   Matches.Where(m => m.PlayerA == playerId).Sum(m => m.PlayerBSets) + Matches.Where(m => m.PlayerB == playerId).Sum(m => m.PlayerASets),
			PointsFor:     Matches.Where(m => m.PlayerA == playerId).Sum(m => m.PlayerATotalPoints) + Matches.Where(m => m.PlayerB == playerId).Sum(m => m.PlayerBTotalPoints),
			PointsAgainst: Matches.Where(m => m.PlayerA == playerId).Sum(m => m.PlayerBTotalPoints) + Matches.Where(m => m.PlayerB == playerId).Sum(m => m.PlayerATotalPoints)
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
