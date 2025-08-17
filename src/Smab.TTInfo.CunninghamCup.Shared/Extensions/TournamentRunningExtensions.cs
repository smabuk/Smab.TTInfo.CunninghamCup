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

			List<Player> shuffledPlayers = [.. tournament.Players.Shuffle()];
			List<Group> groups = [];

			// Distribute players into groups
			List<List<Player>> playerGroupings = [];
			for (int i = 0; i < groupCount; i++)
			{
				playerGroupings.Add([]);
			}

			for (int i = 0; i < shuffledPlayers.Count; i++)
			{
				playerGroupings[i % groupCount].Add(shuffledPlayers[i]);
			}

			for (int i = 0; i < groupCount; i++) {

				groups.Add(Group.Create($"Group {(char)(i + 'A')}", playerGroupings[i]));
			}

			return tournament with { Groups = groups };
		}

	}
}