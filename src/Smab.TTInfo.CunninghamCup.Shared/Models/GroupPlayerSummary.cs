namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a summary of a player's performance within a group, including match results, set counts, and points
/// scored.
/// </summary>
/// <param name="Player">The player whose performance is summarized.</param>
/// <param name="MatchWins">The total number of matches won by the player.</param>
/// <param name="MatchLosses">The total number of matches lost by the player.</param>
/// <param name="SetsFor">The total number of sets won by the player.</param>
/// <param name="SetsAgainst">The total number of sets lost by the player.</param>
/// <param name="PointsFor">The total number of points scored by the player.</param>
/// <param name="PointsAgainst">The total number of points scored against the player.</param>
public record GroupPlayerSummary(
	Player Player,
	int MatchWins, int MatchLosses,
	int SetsFor  , int SetsAgainst,
	int PointsFor, int PointsAgainst
);