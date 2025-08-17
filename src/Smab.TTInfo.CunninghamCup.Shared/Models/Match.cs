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
	public int PlayerASets => Result?.PlayerASets ?? 0;
	public int PlayerBSets => Result?.PlayerBSets ?? 0;
	public int PlayerATotalPoints => Result?.PlayerATotalPoints ?? 0;
	public int PlayerBTotalPoints => Result?.PlayerBTotalPoints ?? 0;

	public bool IsCompleted => Result?.IsCompleted ?? false;
	public string? Notes => Result?.Notes;
	public DateTime? ActualTime => Result?.Sets.Count > 0 ? DateTime.Now : ScheduledTime;
}
