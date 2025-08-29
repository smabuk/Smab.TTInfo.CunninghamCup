namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a match between two players, including their identifiers, starting positions, scheduled time, and the
/// result of the match.
/// </summary>
/// <remarks>A match consists of two players, their starting positions, and an optional scheduled time. The result
/// of the match, if available, determines the winner, the number of sets won by each player, and other related details.
/// The match can also handle scenarios where one or both players are marked as "bye."</remarks>
/// <param name="Id"></param>
/// <param name="PlayerA"></param>
/// <param name="PlayerB"></param>
/// <param name="PlayerAStart"></param>
/// <param name="PlayerBStart"></param>
/// <param name="ScheduledTime"></param>
/// <param name="Result"></param>
public record Match(
	MatchId Id,
	PlayerId PlayerA,
	PlayerId PlayerB,
	int PlayerAStart,
	int PlayerBStart,
	DateTime? ScheduledTime,
	Result? Result
)
{
	public PlayerId? Winner => IsCompleted
		? IsPlayerAWin
			? PlayerA
			: PlayerB
		: null;

	public PlayerId? Loser => IsCompleted
		? IsPlayerAWin
			? PlayerB
			: PlayerA
		: null;

	public bool IsPlayerAWin => (IsCompleted && PlayerB.IsBye) || (Result?.IsPlayerAWin ?? false);
	public bool IsPlayerBWin => (IsCompleted && PlayerA.IsBye) || (Result?.IsPlayerBWin ?? false);
	public int PlayerASets => Result?.PlayerASets ?? 0;
	public int PlayerBSets => Result?.PlayerBSets ?? 0;
	public int PlayerATotalPoints => Result?.PlayerATotalPoints ?? 0;
	public int PlayerBTotalPoints => Result?.PlayerBTotalPoints ?? 0;

	public bool IsCompleted => (Result?.IsCompleted ?? false)
		|| (PlayerA.IsBye && PlayerB.IsPlayer)
		|| (PlayerB.IsBye && PlayerA.IsPlayer)
		|| (PlayerB.IsBye && PlayerA.IsBye);
	public string? Notes => Result?.Notes;
	public DateTime? ActualTime => Result?.Sets.Count > 0 ? DateTime.Now : ScheduledTime;
}
