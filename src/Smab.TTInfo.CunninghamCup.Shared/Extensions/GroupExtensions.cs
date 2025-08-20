namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class GroupExtensions
{
	extension(Group)
	{
		public static Group Create(string name) => new(name, [], []);
		public static Group Create(string name, List<Player> players)
		{
			List<Match> matches = [];

			// Define match orders for group sizes 3-6
			Dictionary<int, List<(int, int)>> matchOrders = new()
			{
				{ 3, [ (1,2), (0,1), (0,2) ] },
				{ 4, [ (0,3), (1,2), (0,1), (2,3), (1,3), (0,2) ] },
				{ 5, [ (1,4), (2,3), (0,1), (2,4), (0,3), (3,4), (0,2), (1,3), (0,4), (1,2) ] },
				{ 6, [ (0,5), (1,4), (2,3), (0,1), (2,4), (3,5), (0,3), (1,2), (4,5), (0,4), (1,3), (2,5), (0,2), (1,5), (3,4) ] }
			};

			int n = players.Count;
			int matchNo = 1;
			if (matchOrders.TryGetValue(n, out List<(int, int)>? order)) {
				foreach ((int a, int b) in order) {
					matches.Add(new Match(
						(MatchId)$"{name} [{matchNo++}]",
						players[a].Id,
						players[b].Id,
						players[a].StartingHandicap(players[b]),
						players[b].StartingHandicap(players[a]),
						null,
						null));
				}
			} else {
				// Fallback: round-robin for other sizes
				for (int a = 0; a < n; a++) {
					for (int b = a + 1; b < n; b++) {
						matches.Add(new Match(
						(MatchId)$"{name} {matchNo++}",
							players[a].Id,
							players[b].Id,
							players[a].StartingHandicap(players[b]),
							players[b].StartingHandicap(players[a]),
							null,
							null));
					}
				}
			}

			return new(name, [.. players.Select(p => p.Id)], [.. matches]);
		}
	}

	extension(Group group)
	{
	}
}