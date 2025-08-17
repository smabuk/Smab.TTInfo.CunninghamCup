namespace Smab.TTInfo.CunninghamCup.Shared.Models;

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

	public static int GetStart(int difference)
	=> _defaultMatrix.TryGetValue(int.Abs(difference), out int start)
		? start
		: 18;

	public static int GetStart(int handicapA, int handicapB)
		=> handicapA <= handicapB
			? 0
			: _defaultMatrix.TryGetValue(handicapA - handicapB, out int start)
				? start
				: 18;
}
