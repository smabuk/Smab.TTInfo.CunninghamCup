namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Result(
	int PlayerAScore,
	int PlayerBScore,
	List<Set> Sets,
	string? Notes
);
