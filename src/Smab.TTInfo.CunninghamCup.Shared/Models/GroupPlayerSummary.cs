namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a summary of a player's performance within a group, including match results and point statistics.
/// </summary>
/// <param name="PlayerId">The unique identifier of the player.</param>
/// <param name="Played">The total number of matches the player has participated in.</param>
/// <param name="MatchWins">The number of matches the player has won.</param>
/// <param name="MatchLosses">The number of matches the player has lost.</param>
/// <param name="SetsFor">The total number of sets won by the player.</param>
/// <param name="SetsAgainst">The total number of sets lost by the player.</param>
/// <param name="PointsFor">The total number of points scored by the player.</param>
/// <param name="PointsAgainst">The total number of points conceded by the player.</param>
public record GroupPlayerSummary(
	PlayerId PlayerId,
	int Played,
	int MatchWins, int MatchLosses,
	int SetsFor  , int SetsAgainst,
	int PointsFor, int PointsAgainst
);