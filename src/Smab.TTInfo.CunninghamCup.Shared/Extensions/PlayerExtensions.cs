using Smab.TTInfo.CunninghamCup.Shared.Models;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class PlayerExtensions
{
	extension(Player)
	{
		public static Player Create(string name) => new(name);
		public static Player Create(string name, int? handicap) => new(name, handicap);
		public static Player Create(string name, int? handicap, int? tteId) => new(name, handicap, tteId);
		public static Player Create(string name, int? handicap, int? tteId, int? ranking) => new(name, handicap, tteId, ranking);
	}

	extension(Player player)
	{
		public int StartingHandicap(Player opponent)
			=> HandicapMatrix.GetStart(player.Handicap ?? 0, opponent.Handicap ?? 0);
	}
}