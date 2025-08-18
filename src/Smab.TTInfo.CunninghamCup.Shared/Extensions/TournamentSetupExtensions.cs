namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class TournamentSetupExtensions
{
	extension(Tournament)
	{
		public static Tournament Create(
			string name,
			DateOnly date
		) => new(Guid.NewGuid(), name, date, [], []);

		public static Tournament Create(
			string name,
			DateOnly date,
			IEnumerable<Player> players
		) => new(Guid.NewGuid(), name, date, [], [..players]);
	}

	extension(Tournament tournament)
	{
		public int GroupsCount => tournament.Groups.Count;
		public int PlayersCount => tournament.Players.Count;

		public bool HasGroups => tournament.Groups.Count > 0;

		/// <summary>
		/// Adds a new player to the tournament or updates an existing player's details.
		/// </summary>
		/// <remarks>If a player with the specified <paramref name="name"/> does not exist, a new player is created
		/// and added to the tournament. If a player with the specified <paramref name="name"/> already exists, their details
		/// are updated with the provided values. Any null values for <paramref name="handicap"/>, <paramref name="tteId"/>,
		/// or <paramref name="ranking"/> will result in the corresponding existing values being retained.</remarks>
		/// <param name="name">The name of the player. This value is used to identify the player and cannot be null or empty.</param>
		/// <param name="handicap">The player's handicap value. If null, the existing handicap value is retained when updating an existing player.</param>
		/// <param name="tteId">The player's TTE (Table Tennis England) ID. If null, the existing TTE ID is retained when updating an existing
		/// player.</param>
		/// <param name="ranking">The player's ranking. If null, the existing ranking value is retained when updating an existing player.</param>
		public void AddOrUpdatePlayer(string name, int? handicap = null, int? tteId = null, int? ranking = null)
		{
			const int NotFound = -1;

			int ix = tournament.Players.FindIndex(p => p.Name == name);
			if (ix is NotFound) {
				tournament.Players.Add(Player.Create(name, handicap, tteId, ranking));
			} else {
				Player? player = tournament.Players[ix];
				handicap ??= player.Handicap;
				tteId    ??= player.TTEId;
				ranking  ??= player.Ranking;
				tournament.Players[ix] = player with { Handicap = handicap, TTEId = tteId, Ranking = ranking };
			}
		}
	}
}