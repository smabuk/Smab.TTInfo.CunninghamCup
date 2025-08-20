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
		/// <summary>
		/// Generates a detailed string representation of the group, including its players, matches, and player summaries.
		/// </summary>
		/// <remarks>The returned string includes the group's name, a list of players with their handicaps, a list of
		/// matches with player details and starting handicaps,  and a summary of each player's performance within the group.
		/// This method is useful for creating a human-readable overview of the group's state.</remarks>
		/// <returns>A string containing a formatted representation of the group's details, including players, matches, and performance
		/// summaries.</returns>
		public string AsString(Tournament tournament)
		{
			StringBuilder sb = new();
			_ = sb.AppendLine($"Group: {group.Name}");
			_ = sb.AppendLine($"Players ({group.Players.Count}):");
			foreach (PlayerId playerId in group.Players) {
				_ = sb.AppendLine($"- {playerId.DisplayName,-12} ({tournament.GetPlayer(playerId).Handicap,3})");
			}

			_ = sb.AppendLine();
			_ = sb.AppendLine($"Matches ({group.Matches.Count}):");
			foreach (Match match in group.Matches) {
				_ = sb.AppendLine($"- {match.PlayerA.DisplayName,-12} ({match.PlayerAStart,3}) vs {match.PlayerB.DisplayName,-12} ({match.PlayerBStart,3})  Id: {match.Id}");
			}

			_ = sb.AppendLine();
			foreach (GroupPlayerSummary groupPlayerSummary in group.GroupPositions) {
				_ = sb.Append($"- {groupPlayerSummary.PlayerId.DisplayName,-12}");
				_ = sb.Append($"  W: {groupPlayerSummary.MatchWins,2}, L: {groupPlayerSummary.MatchLosses,2}");
				_ = sb.Append($"  SF: {groupPlayerSummary.SetsFor,2}, SA: {groupPlayerSummary.SetsAgainst,2}");
				_ = sb.Append($"  PF: {groupPlayerSummary.PointsFor,3}, PA: {groupPlayerSummary.PointsAgainst,3}");
				_ = sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}