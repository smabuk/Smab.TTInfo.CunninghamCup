using System.Text;

namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;

public static class GroupExtensions
{
	extension(Group group)
	{
		public Group CompleteWithRandomResults()
		{
			List<Match> matches = [.. group.Matches];
			for (int i = 0; i < matches.Count; i++) {
				if (!matches[i].IsCompleted) {
					matches[i] = matches[i].SetRandomResult();
				}
			}

			return group with { Matches = [.. matches] };
		}

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
