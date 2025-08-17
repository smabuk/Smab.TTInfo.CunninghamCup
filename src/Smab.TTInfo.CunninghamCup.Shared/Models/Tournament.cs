namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Tournament(
	string Name,
	DateTime Date,
	List<Group> Groups,
	List<PlayerEntry> Players
);

