namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record PlayerEntry(
	Player Player,
	int? Handicap = null,
	bool WithDrawn = false
);
