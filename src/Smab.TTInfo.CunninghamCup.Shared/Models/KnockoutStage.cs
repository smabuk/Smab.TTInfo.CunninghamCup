namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record KnockoutStage(
	string Name, // e.g., "Knockout Stage"
	List<PlayerEntry> Players,
	List<KnockoutRound> Rounds
);
