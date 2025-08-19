namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class PlayerExtensions
{
	extension(Player)
	{
		public static Player Create(string name) => new((PlayerId)name, name);
		public static Player Create(string name, int? handicap) => new((PlayerId)name, name, handicap);
		public static Player Create(string name, int? handicap, int? tteId) => new((PlayerId)name, name, handicap, tteId);
		public static Player Create(string name, int? handicap, int? tteId, int? ranking) => new((PlayerId)name, name, handicap, tteId, ranking);
	}

	extension(Player player)
	{
		/// <summary>
		/// Calculates the starting handicap for the current player based on the opponent's handicap.
		/// </summary>
		/// <param name="opponent">The opponent player whose handicap is used in the calculation. Cannot be null.</param>
		/// <returns>The starting handicap for the current player as an integer.</returns>
		public int StartingHandicap(Player opponent)
			=> HandicapMatrix.GetStart(player.Handicap ?? 0, opponent.Handicap ?? 0);
	}
}