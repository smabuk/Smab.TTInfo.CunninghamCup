namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Result(
	List<Set> Sets,
	string? Notes
)
{
	public int PlayerASets => Sets.Count(s => s.IsPlayerAWin());
	public int PlayerATotalPoints => Sets.Sum(s => s.PlayerAScore);

	public int PlayerBSets => Sets.Count(s => s.IsPlayerBWin());
	public int PlayerBTotalPoints => Sets.Sum(s => s.PlayerBScore);

	public bool IsPlayerAWin => IsCompleted && PlayerASets > PlayerBSets;
	public bool IsPlayerBWin => IsCompleted && PlayerBSets > PlayerASets;

	public bool IsCompleted => PlayerASets >= 2 || PlayerBSets >= 2;
}
