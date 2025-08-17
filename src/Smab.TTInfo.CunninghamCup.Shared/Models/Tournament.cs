namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Tournament(
	Guid Id,
	string Name,
	DateTime Date,
	List<Group> Groups,
	List<PlayerEntry> Players,
	KnockoutStage? KnockoutStage = null
);

