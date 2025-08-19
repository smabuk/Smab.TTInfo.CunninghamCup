namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class TournamentRunningExtensions
{
	extension(Tournament tournament)
	{
		/// <summary>
		/// Distributes players into groups for the tournament based on the specified group size.
		/// </summary>
		/// <remarks>This method shuffles the players randomly before assigning them to groups. The groups are named
		/// sequentially using letters (e.g., "Group A", "Group B", etc.). If the number of players is not evenly divisible by
		/// the group size, some groups may have fewer players.</remarks>
		/// <param name="groupSize">The maximum number of players allowed in each group. Must be greater than zero.</param>
		/// <returns>A new <see cref="Tournament"/> instance with players distributed into groups. The number of groups is determined
		/// by the total number of players and the specified group size.</returns>
		/// <exception cref="InvalidOperationException">Thrown if the tournament already has groups drawn.</exception>
		public Tournament DrawGroups(int groupSize)
		{
			int groupCount = (int)Math.Ceiling((double)tournament.ActivePlayers.Count / groupSize);
			if (tournament.Groups.Count > 0)
			{
				throw new InvalidOperationException("Cannot draw groups for a tournament with groups already drawn.");
			}

			List<Player> shuffledPlayers = [.. tournament.ActivePlayers.Shuffle()];
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