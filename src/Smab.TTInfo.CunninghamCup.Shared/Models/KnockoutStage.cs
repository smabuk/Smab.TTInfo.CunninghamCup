namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record KnockoutStage(
	string Name, // e.g., "Knockout Stage"
	List<Player> Players,
	List<KnockoutRound> Rounds
);
