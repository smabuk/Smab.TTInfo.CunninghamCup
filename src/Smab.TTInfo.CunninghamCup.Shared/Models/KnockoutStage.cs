namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record KnockoutStage(string Name, List<int> GroupPositions, List<KnockoutRound> Rounds);
