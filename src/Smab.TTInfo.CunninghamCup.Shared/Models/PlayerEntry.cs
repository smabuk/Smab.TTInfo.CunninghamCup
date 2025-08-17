namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record PlayerEntry(
	Player Player,
	int Handicap,
	bool WithDrawn = false
);
