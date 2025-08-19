namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a player with associated details such as ID, name, and optional attributes like handicap, ranking, and
/// withdrawal status.
/// </summary>
/// <param name="Id">The unique identifier for the player.</param>
/// <param name="Name">The name of the player.</param>
/// <param name="Handicap">The player's handicap, if available. A null value indicates no handicap is specified.</param>
/// <param name="TTEId">The player's Table Tennis England (TTE) identifier, if available. A null value indicates no TTE ID is specified.</param>
/// <param name="Ranking">The player's ranking, if available. A null value indicates no ranking is specified.</param>
/// <param name="WithDrawn">A value indicating whether the player has withdrawn. <see langword="true"/> if the player has withdrawn; otherwise,
/// <see langword="false"/>.</param>
public record Player(
	PlayerId Id,
	string Name,
	int? Handicap = null,
	int? TTEId = null,
	int? Ranking = null,
	bool WithDrawn = false
);
