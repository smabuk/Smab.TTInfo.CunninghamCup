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
		) => new(Guid.NewGuid(), name, date, [], players.ToDictionary(p => p.Id, p => p));
	}

	extension(Tournament tournament)
	{
		public int GroupsCount => tournament.Groups.Count;
		public int PlayersCount => tournament.Players.Count;

		public bool HasGroups => tournament.Groups.Count > 0;

		public List<Player> ActivePlayers => [.. tournament.Players.Values.Where(p => p.WithDrawn is false)];
		public Player GetPlayer(PlayerId playerId) => tournament.Players[playerId];

		/// <summary>
		/// Adds a new player to the tournament or updates an existing player's details.
		/// </summary>
		/// <remarks>If a player with the specified <paramref name="playerId"/> does not exist, a new player is created
		/// and added to the tournament. If a player with the specified <paramref name="playerId"/> already exists, their details
		/// are updated with the provided values. Any null values for <paramref name="handicap"/>, <paramref name="tteId"/>,
		/// or <paramref name="ranking"/> will result in the corresponding existing values being retained.</remarks>
		/// <param name="playerId">The name of the player. This value is used to identify the player and cannot be null or empty.</param>
		/// <param name="handicap">The player's handicap value. If null, the existing handicap value is retained when updating an existing player.</param>
		/// <param name="tteId">The player's TTE (Table Tennis England) ID. If null, the existing TTE ID is retained when updating an existing
		/// player.</param>
		/// <param name="ranking">The player's ranking. If null, the existing ranking value is retained when updating an existing player.</param>
		public void AddOrUpdatePlayer(PlayerId playerId, int? handicap = null, int? tteId = null, int? ranking = null)
		{
			if (tournament.Players.TryGetValue(playerId, out Player? existingPlayer)) {
				handicap ??= existingPlayer.Handicap;
				tteId    ??= existingPlayer.TTEId;
				ranking  ??= existingPlayer.Ranking;
				tournament.Players[playerId] = existingPlayer with { Handicap = handicap, TTEId = tteId, Ranking = ranking };
			} else {
				tournament.Players.Add(playerId, Player.Create(playerId, handicap, tteId, ranking));
			}
		}

		public void AddOrUpdatePlayer(string name, int? handicap = null, int? tteId = null, int? ranking = null)
			=> tournament.AddOrUpdatePlayer((PlayerId)name, handicap, tteId, ranking);

		public void AddOrUpdatePlayer(Player player) => tournament.Players[player.Id] = player;
	}
}