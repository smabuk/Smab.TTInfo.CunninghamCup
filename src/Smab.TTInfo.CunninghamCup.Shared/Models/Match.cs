namespace Smab.TTInfo.CunninghamCup.Shared.Models;

public record Match(
	Player PlayerA,
	Player PlayerB,
	DateTime? ScheduledTime,
	Result? Result
)
{
	public bool IsPlayerAWin => Result?.IsPlayerAWin ?? false;
	public bool IsPlayerBWin => Result?.IsPlayerBWin ?? false;
	public bool IsCompleted => Result?.IsCompleted ?? false;
	public string? Notes => Result?.Notes;
	public DateTime? ActualTime => Result?.Sets.Count > 0 ? DateTime.Now : ScheduledTime;
}
