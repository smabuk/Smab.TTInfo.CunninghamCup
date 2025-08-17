namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Player(
	string Name,
	int? TTEId = null,
	int? Ranking = null
);
