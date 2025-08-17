namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Tournament(
	Guid Id,
	string Name,
	DateTime Date,
	List<Group> Groups,
	List<Player> Players,
	KnockoutStage? KnockoutStage = null
);

