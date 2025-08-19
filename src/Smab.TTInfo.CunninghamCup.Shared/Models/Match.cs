namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents a match between two players, including details about the players, the scheduled time,  and the result of
/// the match.
/// </summary>
/// <remarks>This record encapsulates the key information about a match, such as the players involved,  the
/// scheduled time, and the result (if available). It also provides derived properties  to access specific details about
/// the match outcome, such as the number of sets won by each player  and whether the match is completed.</remarks>
/// <param name="PlayerA">The first player participating in the match.</param>
/// <param name="PlayerB">The second player participating in the match.</param>
/// <param name="ScheduledTime">The scheduled date and time for the match. Can be <see langword="null"/> if not set.</param>
/// <param name="Result">The result of the match, if available. Can be <see langword="null"/> if the match has not been completed.</param>
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
