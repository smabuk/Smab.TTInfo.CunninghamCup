namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class TournamentExtensions
{
	extension(Tournament tournament)
	{

		public bool GroupsCompleted => tournament.Groups.All(g => g.IsCompleted);


		/// <summary>
		/// Calculates the starting handicap values for two players in a tournament.
		/// </summary>
		/// <param name="playerAId">The unique identifier of the first player.</param>
		/// <param name="playerBId">The unique identifier of the second player.</param>
		/// <returns>A tuple containing the starting handicap values for both players: <see cref="ValueTuple.Item1"/> is the starting
		/// handicap for player A against player B,  and <see cref="ValueTuple.Item2"/> is the starting handicap for player B
		/// against player A.</returns>
		/// <exception cref="ArgumentException">Thrown if either <paramref name="playerAId"/> or <paramref name="playerBId"/> does not correspond  to a player in
		/// the tournament.</exception>
		public (int PlayerAStart, int PlayerBStart) StartingHandicap(PlayerId playerAId, PlayerId playerBId)
		{
			if (tournament.Players.TryGetValue(playerAId, out Player? playerA) &&
				tournament.Players.TryGetValue(playerBId, out Player? playerB))
			{
				return (playerA.StartingHandicap(playerB), playerB.StartingHandicap(playerA));
			}

			throw new ArgumentException("Player or opponent not found in tournament.");
		}
	}
}
