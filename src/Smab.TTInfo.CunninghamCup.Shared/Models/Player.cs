namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Player(
	string Name,
	int? Handicap = null,
	int? TTEId = null,
	int? Ranking = null,
	bool WithDrawn = false
);
