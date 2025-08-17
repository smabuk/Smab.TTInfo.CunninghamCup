namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record GroupPlayerSummary(
	Player Player,
	int MatchWins, int MatchLosses,
	int SetsFor  , int SetsAgainst,
	int PointsFor, int PointsAgainst
);