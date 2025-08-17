using Smab.TTInfo.CunninghamCup.Shared.Models;

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