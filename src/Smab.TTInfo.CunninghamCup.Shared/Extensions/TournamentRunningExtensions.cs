using System.Diagnostics.CodeAnalysis;

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

		public bool TryDrawKnockoutStage(out Tournament newTournament, [NotNullWhen(false)] out string? message)
		{
			newTournament = tournament;
			message = "";
			KnockoutStage? knockoutStage = tournament.KnockoutStage;
			if (knockoutStage is not null)
			{
				message = "Cannot draw knockout stage for a tournament with knockout stage already drawn.";
				return false;
			}

			if (tournament.GroupsCompleted is false)
			{
				message = "Cannot draw knockout stage for a tournament with incomplete groups.";
				return false;
			}

			int noOfRounds = (tournament.GroupsCount, tournament.ActivePlayers.Count) switch
			{
				(1,    _) => 0,
				(2,    _) => 1,
				(3,    _) => 2,
				(4,    _) => 2,
				(5,    _) => 3,
				(6,    _) => 2,
				(7, < 24) => 3,
				(7,    _) => 4,
				(8, < 24) => 3,
				(8,    _) => 4,
				_ => 0
			};

			List<KnockoutRound> knockoutRounds = [];
			int matchNo = 0;
			for (int round = noOfRounds; round > 0;round--)
			{
				int matchesPerRound = (int)Math.Pow(2, round) / 2;
				List<Match> matches = [];
				string roundName = round switch
				{
					1 => $"Final",
					2 => $"Semi-Final",
					3 => $"Quarter-Final",
					4 => $"Round of 16",
					5 => $"Round of 32",
					_ => $"Round {noOfRounds - round + 1}"
				};
				for (int i = 0; i < matchesPerRound; i++)
				{
					matchNo++;
					matches.Add(new Match(
						(MatchId)$"{roundName} {matchNo}",
						(PlayerId)$"|{matchNo - (matchesPerRound * 2) + i}",
						(PlayerId)$"|{matchNo - (matchesPerRound * 2) + i + 1}",
						0,
						0,
						null,
						null));

				}

				knockoutRounds.Add(new KnockoutRound($"{roundName}", [..matches]));
			}

			knockoutStage = new KnockoutStage("Knockout", [], knockoutRounds);



			newTournament = tournament with { KnockoutStage = knockoutStage };
			return true;
		}

	}
}