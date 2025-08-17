using Smab.TTInfo.CunninghamCup.Shared.Models;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class TournamentRunningExtensions
{
	extension(Tournament tournament)
	{
		public Tournament DrawGroups(int groupSize)
		{
			int groupCount = (int)Math.Ceiling((double)tournament.Players.Count / groupSize);
			if (tournament.Groups.Count > 0)
			{
				throw new InvalidOperationException("Cannot draw groups for a tournament with groups already drawn.");
			}

			List<Group> groups = [];
			for (int i = 0; i < groupCount; i++) {
				groups.Add(new($"Group {(char)(i + 32)}", [], []));
			}

			List<Player> shuffledPlayers = [.. tournament.Players.Shuffle()];
			for (int i = 0; i < shuffledPlayers.Count; i++)
			{
				groups[i % groupCount].Players.Add(shuffledPlayers[i]);
			}

			return tournament with { Groups = groups };
		}

	}
}