using System.ComponentModel.DataAnnotations;

namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Represents the score of a set in a game, including the scores of Player A and Player B.
/// </summary>
/// <remarks>A set is defined by the scores of two players, Player A and Player B, with constraints on the valid
/// score range. Use the provided methods to determine the outcome of the set, such as whether Player A or Player B has
/// won, or if the set is a draw.</remarks>
/// <param name="PlayerAScore"></param>
/// <param name="PlayerBScore"></param>
public record Set(
	[Range(0, 30)] int PlayerAScore,
	[Range(0, 30)] int PlayerBScore
)
{
	public bool IsPlayerAWin => PlayerAScore > PlayerBScore;
	public bool IsPlayerBWin => PlayerBScore > PlayerAScore;
	public bool IsDraw => PlayerAScore == PlayerBScore;
}
