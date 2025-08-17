using Smab.TTInfo.CunninghamCup.Shared.Models;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class HandicapMatrixExtensions
{
	extension(HandicapMatrix)
	{
		public static int GetStart(int difference)
			=> HandicapMatrix.DefaultMatrix.TryGetValue(int.Abs(difference), out var start)
				? start
				: 18;

		public static int GetStart(int handicapA, int handicapB)
			=> HandicapMatrix.DefaultMatrix.TryGetValue(int.Abs(handicapA - handicapB), out var start)
				? start
				: 18;
	}
}