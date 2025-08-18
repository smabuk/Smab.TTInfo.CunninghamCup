namespace Smab.TTInfo.CunninghamCup.Shared.Models;

/// <summary>
/// Provides methods for determining the starting point in a handicap-based system based on the difference between two
/// handicap values.
/// </summary>
/// <remarks>This class is designed to calculate starting values for scenarios where a handicap difference is used
/// to determine an initial position or advantage. The calculations are based on a predefined matrix of handicap
/// differences and their corresponding starting values.</remarks>
public sealed class HandicapMatrix
{
	internal readonly static IReadOnlyDictionary<int, int> _defaultMatrix =
		new Dictionary<int, int>
		{
			{ 00, 00 },
			{ 01, 01 },
			{ 02, 02 },
			{ 03, 03 },
			{ 04, 04 },
			{ 05, 05 },
			{ 06, 06 },
			{ 07, 07 },
			{ 08, 08 },
			{ 09, 09 },
			{ 10, 10 },
			{ 11, 11 },
			{ 12, 12 },
			{ 13, 12 },
			{ 14, 13 },
			{ 15, 13 },
			{ 16, 14 },
			{ 17, 14 },
			{ 18, 15 },
			{ 19, 15 },
			{ 20, 16 },
			{ 21, 16 },
			{ 22, 17 },
			{ 23, 17 },
			{ 24, 17 },
			{ 25, 18 },
			{ 26, 18 },
			{ 27, 18 },
			{ 28, 18 },
			{ 29, 18 }
		};

	/// <summary>
	/// Retrieves the starting value associated with the specified difference.
	/// </summary>
	/// <remarks>This method uses a predefined matrix to map absolute difference values to starting values.  If the
	/// specified difference is not found in the matrix, a default value of 18 is returned.</remarks>
	/// <param name="difference">The difference value for which to retrieve the starting value. Must be a non-negative integer.</param>
	/// <returns>The starting value corresponding to the absolute value of <paramref name="difference"/> if found;  otherwise,
	/// returns 18.</returns>
	public static int GetStart(int difference)
	=> _defaultMatrix.TryGetValue(int.Abs(difference), out int start)
		? start
		: 18;

	/// <summary>
	/// Determines the starting point based on the difference between two handicaps.
	/// </summary>
	/// <param name="handicapA">The handicap of the first player. Must be a non-negative integer.</param>
	/// <param name="handicapB">The handicap of the second player. Must be a non-negative integer.</param>
	/// <returns>The starting point as an integer. Returns 0 if <paramref name="handicapA"/> is less than or equal to  <paramref
	/// name="handicapB"/>. If the difference between the handicaps exists in the predefined matrix,  the corresponding
	/// value is returned; otherwise, returns 18.</returns>
	public static int GetStart(int handicapA, int handicapB)
		=> handicapA <= handicapB
			? 0
			: _defaultMatrix.TryGetValue(handicapA - handicapB, out int start)
				? start
				: 18;
}
