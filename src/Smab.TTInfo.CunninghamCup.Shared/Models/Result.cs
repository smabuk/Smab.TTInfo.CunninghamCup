namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents the result of a match, including the sets played, player scores, and match outcome.
/// </summary>
/// <remarks>This record encapsulates the details of a match, including the sets played, notes about the match, 
/// and computed properties to determine the winner and total points for each player. A match is considered  completed
/// when either player has won at least two sets.</remarks>
/// <param name="Sets"></param>
/// <param name="Notes"></param>
public record Result(
	List<Set> Sets,
	string? Notes
)
{
	public int PlayerASets => Sets.Count(s => s.IsPlayerAWin);
	public int PlayerATotalPoints => Sets.Sum(s => s.PlayerAScore);

	public int PlayerBSets => Sets.Count(s => s.IsPlayerBWin);
	public int PlayerBTotalPoints => Sets.Sum(s => s.PlayerBScore);

	public bool IsPlayerAWin => IsCompleted && PlayerASets > PlayerBSets;
	public bool IsPlayerBWin => IsCompleted && PlayerBSets > PlayerASets;

	public bool IsCompleted => PlayerASets >= 2 || PlayerBSets >= 2;
	public bool IsNotCompleted => !IsCompleted;

	public override string ToString()
	{
		// Join sets as "A-B"
		string setsJoined = Sets is { Count: > 0 }
			? string.Join(", ", Sets.Select(s => $"{s.PlayerAScore}-{s.PlayerBScore}"))
			: "no sets";

		// Summary components
		string setsScore = $"{PlayerASets}-{PlayerBSets}";
		string points = $"{PlayerATotalPoints}-{PlayerBTotalPoints}";

		// Outcome
		string outcome = IsCompleted
			? (IsPlayerAWin ? "Player A wins" : IsPlayerBWin ? "Player B wins" : "Draw")
			: "Incomplete";

		// Notes
		string notesPart = string.IsNullOrWhiteSpace(Notes) ? string.Empty : $" Notes: {Notes}";

		return $$"""{{nameof(Result)}} { {{outcome}} [{{setsJoined}}]; Sets: {{setsScore}}; Points: {{points}}; {{notesPart}} }""";
	}
}
