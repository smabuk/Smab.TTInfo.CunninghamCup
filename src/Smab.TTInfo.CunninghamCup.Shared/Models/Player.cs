namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a player in a competition or ranking system.
/// </summary>
/// <param name="Name">The name of the player. This value cannot be null.</param>
/// <param name="Handicap">The player's handicap, if applicable. A null value indicates no handicap is assigned.</param>
/// <param name="TTEId">The player's unique identifier in the Table Tennis England (TTE) system, if available. A null value indicates no TTE
/// ID is assigned.</param>
/// <param name="Ranking">The player's ranking position, if applicable. A null value indicates no ranking is assigned.</param>
/// <param name="WithDrawn">A value indicating whether the player has withdrawn from the competition. <see langword="true"/> if the player has
/// withdrawn; otherwise, <see langword="false"/>.</param>
public record Player(
	string Name,
	int? Handicap = null,
	int? TTEId = null,
	int? Ranking = null,
	bool WithDrawn = false
);
