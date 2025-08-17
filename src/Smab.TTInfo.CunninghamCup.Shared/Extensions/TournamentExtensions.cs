using Smab.TTInfo.CunninghamCup.Shared.Models;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class TournamentExtensions
{
	extension(Tournament)
	{
		public static Tournament Create(
			string name,
			DateTime date
		) => new(Guid.CreateVersion7(), name, date, [], []);

		public static Tournament Create(
			string name,
			DateTime date,
			IEnumerable<PlayerEntry> players
		) => new(Guid.CreateVersion7(), name, date, [], [..players]);
	}

	extension(Tournament tournament)
	{
		public int GroupsCount => tournament.Groups.Count;
		public int PlayersCount => tournament.Players.Count;

		public bool HasGroups => tournament.Groups.Count > 0;

		public void AddOrUpdatePlayer(string name, int? handicap = null)
		{
			const int NotFoundIndex = -1;

			int ix = tournament.Players.FindIndex(p => p.Player.Name == name);
			if (ix == NotFoundIndex) {
				tournament.Players.Add(new(new(name), handicap));
			} else {
				tournament.Players[ix] = tournament.Players[ix] with { Handicap = handicap };
			}
		}
	}
}