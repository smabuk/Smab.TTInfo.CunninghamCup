namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Result(
	List<Set> Sets,
	string? Notes
)
{
	public int PlayerAScore => Sets.Count(s => s.IsPlayerAWin());
	public int PlayerBScore => Sets.Count(s => s.IsPlayerBWin());

	public bool IsPlayerAWin => IsCompleted && PlayerAScore > PlayerBScore;
	public bool IsPlayerBWin => IsCompleted && PlayerBScore > PlayerAScore;

	public bool IsCompleted => PlayerAScore >= 2 || PlayerBScore >= 2;
}
