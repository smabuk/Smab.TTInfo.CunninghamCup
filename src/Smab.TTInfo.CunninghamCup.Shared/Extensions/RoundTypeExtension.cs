namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class RoundTypeExtension
{
	extension(RoundType roundType)
	{
		/// <summary>
		/// Converts the RoundType to a user-friendly string representation.
		/// </summary>
		/// <returns>A string that represents the RoundType in a user-friendly format.</returns>
		public string ToFriendlyString()
		{
			return roundType switch
			{
				RoundType.Final        => "Final",
				RoundType.SemiFinal    => "Semi-Final",
				RoundType.QuarterFinal => "Quarter-Final",
				RoundType.RoundOf16    => "Round of 16",
				RoundType.RoundOf32    => "Round of 32",
				RoundType.RoundOf64    => "Round of 64",
				RoundType.RoundOf128   => "Round of 128",
				RoundType.ThirdPlacePlayoff => "Third Place Playoff",
				_ => ""
			};
		}
	}
}
