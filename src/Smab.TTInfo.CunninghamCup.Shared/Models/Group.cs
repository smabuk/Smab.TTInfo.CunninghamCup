namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Group(
	string Name,
	List<PlayerEntry> Players,
	List<Match> Matches
);
