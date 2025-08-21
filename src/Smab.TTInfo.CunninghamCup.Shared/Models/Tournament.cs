namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a tournament, including its identifier, name, date, groups, players, and optional knockout stage.
/// </summary>
/// <remarks>A tournament consists of multiple groups and players, with an optional knockout stage to determine
/// the final winner. The <see cref="Groups"/> property contains the groups participating in the tournament, while the
/// <see cref="Players"/> property maps player identifiers to their corresponding player details. The <see
/// cref="KnockoutStage"/> property, if not null, represents the knockout stage of the tournament.</remarks>
/// <param name="Id">The unique identifier of the tournament.</param>
/// <param name="Name">The name of the tournament.</param>
/// <param name="Date">The date on which the tournament takes place.</param>
/// <param name="Groups">The list of groups participating in the tournament.</param>
/// <param name="Players">A dictionary mapping player identifiers to their corresponding player details.</param>
/// <param name="KnockoutStage">The optional knockout stage of the tournament. If null, the tournament does not include a knockout stage.</param>
public record Tournament(
	Guid Id,
	string Name,
	DateOnly Date,
	List<Group> Groups,
	Dictionary<PlayerId, Player> Players,
	KnockoutStage? KnockoutStage = null
);
