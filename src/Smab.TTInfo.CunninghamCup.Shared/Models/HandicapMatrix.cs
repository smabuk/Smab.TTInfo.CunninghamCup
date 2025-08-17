namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record HandicapMatrix(
	IReadOnlyList<HandicapMatrixEntry> PlayerHandicaps
);
