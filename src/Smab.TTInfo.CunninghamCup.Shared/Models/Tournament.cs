namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a tournament, including its details, participants, and stages.
/// </summary>
/// <remarks>A tournament consists of a unique identifier, a name, a date, and collections of groups and players.
/// Optionally, it may include a knockout stage. This record is immutable and provides a concise way to encapsulate
/// tournament data.</remarks>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Date"></param>
/// <param name="Groups"></param>
/// <param name="Players"></param>
/// <param name="KnockoutStage"></param>
public record Tournament(
	Guid Id,
	string Name,
	DateOnly Date,
	List<Group> Groups,
	Dictionary<PlayerId, Player> Players,
	KnockoutStage? KnockoutStage = null
);
