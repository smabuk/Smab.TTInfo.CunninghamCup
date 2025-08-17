namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Group(
	string Name,
	List<Player> Players,
	List<Match> Matches
);
