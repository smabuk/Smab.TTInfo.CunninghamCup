namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class MatchExtensions
{
	extension(Match)
	{
	}

	extension(Match match)
	{
		/// <summary>
		/// Sets the result of the match using the provided sets.
		/// </summary>
		/// <remarks>This method creates a new result for the match based on the provided sets and returns a new match
		/// instance with the updated result. The original match instance remains unchanged.</remarks>
		/// <param name="sets">A collection of sets that define the result of the match. Each set represents a portion of the match's outcome.</param>
		/// <returns>A new <see cref="Match"/> instance with the updated result.</returns>
		public Match SetResult(params IEnumerable<Set> sets)
		{
			if (match.PlayerA.IsPlaceHolder || match.PlayerB.IsPlaceHolder) {
				throw new InvalidOperationException("Cannot set the result of a match with placeholder players.");
			}

			Result result = new([.. sets], null);
			return match with { Result = result };
		}

		/// <summary>
		/// Sets the result of the match using the provided sets of scores.
		/// </summary>
		/// <remarks>This method creates a new match instance with the specified result, leaving the original match
		/// instance unchanged.</remarks>
		/// <param name="sets">A collection of tuples where each tuple represents the scores of Player A and Player B for a set. Each tuple must
		/// contain the score for Player A as the first element and the score for Player B as the second element.</param>
		/// <returns>A new <see cref="Match"/> instance with the updated result.</returns>
		public Match SetResult(params IEnumerable<(int PlayerAScore, int PlayerBScore)> sets)
		{
			if (match.PlayerA.IsPlaceHolder || match.PlayerB.IsPlaceHolder) {
				throw new InvalidOperationException("Cannot set the result of a match with placeholder players.");
			}

			Result result = new([.. sets.Select(s => new Set(s.PlayerAScore, s.PlayerBScore))], null);
			return match with { Result = result };
		}
	}
}