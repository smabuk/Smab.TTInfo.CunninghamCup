using System.Text;

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
			Dictionary<int, int[,]> matchOrders = new()
			{
				{ 3, new[,] { {1,2}, {0,1}, {0,2} } },
				{ 4, new[,] { {0,3}, {1,2}, {0,1}, {2,3}, {1,3}, {0,2} } },
				{ 5, new[,] { {1,4}, {2,3}, {0,1}, {2,4}, {0,3}, {3,4}, {0,2}, {1,3}, {0,4}, {1,2} } },
				{ 6, new[,] { {0,5}, {1,4}, {2,3}, {0,1}, {2,4}, {3,5}, {0,3}, {1,2}, {4,5}, {0,4}, {1,3}, {2,5}, {0,2}, {1,5}, {3,4} } }
			};

			int n = players.Count;
			if (matchOrders.TryGetValue(n, out int[,]? order)) {
				for (int i = 0; i < order.GetLength(0); i++) {
					int a = order[i, 0];
					int b = order[i, 1];
					matches.Add(new Match(players[a], players[b], null, null));
				}
			} else {
				// Fallback: round-robin for other sizes
				for (int i = 0; i < n; i++) {
					for (int j = i + 1; j < n; j++) {
						matches.Add(new Match(players[i], players[j], null, null));
					}
				}
			}

			return new(name, players, matches);
		}
	}

	extension(Group group)
	{
		public string AsString()
		{
			StringBuilder sb = new();
			_ = sb.AppendLine($"Group: {group.Name}");
			_ = sb.AppendLine($"Players ({group.Players.Count}):");
			foreach (Player player in group.Players) {
				_ = sb.AppendLine($"- {player.Name,-12} ({player.Handicap,3})");
			}

			_ = sb.AppendLine();
			_ = sb.AppendLine($"Matches ({group.Matches.Count}):");
			foreach (Match match in group.Matches) {
				_ = sb.AppendLine($"- {match.PlayerA.Name,-12} ({match.PlayerA.StartingHandicap(match.PlayerB),3}) vs {match.PlayerB.Name,-12} ({match.PlayerB.StartingHandicap(match.PlayerA),3})");
			}

			_ = sb.AppendLine();
			foreach (GroupPlayerSummary groupPlayerSummary in group.GroupPositions) {
				_ = sb.Append($"- {groupPlayerSummary.Player.Name,-12}");
				_ = sb.Append($"  W: {groupPlayerSummary.MatchWins,2}, L: {groupPlayerSummary.MatchLosses,2}");
				_ = sb.Append($"  SF: {groupPlayerSummary.SetsFor,2}, SA: {groupPlayerSummary.SetsAgainst,2}");
				_ = sb.Append($"  PF: {groupPlayerSummary.PointsFor,2}, PA: {groupPlayerSummary.PointsAgainst,2}");
				_ = sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}