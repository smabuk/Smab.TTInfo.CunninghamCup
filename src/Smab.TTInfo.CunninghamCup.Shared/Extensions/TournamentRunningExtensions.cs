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
			if (knockoutStage is null)
			{
				int noOfRounds = (tournament.GroupsCount, tournament.ActivePlayers.Count) switch
				{
					(1, _)    => 0,
					(_, > 23) => 4,
					(_, > 16) => 3,
					(_, > 8)  => 2,
					(8, _)    => 4,
					_         => 0
				};

				List<KnockoutRound> knockoutRounds = [];
				int matchNo = 0;
				for (int round = 0; round < noOfRounds; round++) {
					int matchesPerRound = (int)Math.Pow(2, noOfRounds - round) / 2;
					List<Match> matches = [];
					string roundName = (noOfRounds - round) switch
					{
						1 => $"Final",
						2 => $"Semi-Final",
						3 => $"Quarter-Final",
						4 => $"Round of 16",
						5 => $"Round of 32",
						_ => $"Round {round + 1}"
					};

					int noOfGroups = tournament.GroupsCount;

					List<int> winnerPositions   = [.. Enumerable.Range(0, noOfGroups).Shuffle()];
					List<int> runnerUpPositions = [.. Enumerable.Range(0, noOfGroups).Shuffle()];

					while (winnerPositions.Zip(runnerUpPositions).Any(p => p.First == p.Second)) {
						runnerUpPositions = [.. runnerUpPositions.Shuffle()];
					}

					List<int> firstRoundPlacements = [.. Enumerable.Range(0, matchesPerRound).Shuffle()];

					for (int i = 0; i < matchesPerRound; i++) {
						matchNo++;
						if (round == 0) {
							int drawPlacement = firstRoundPlacements[i];
							// For the first round, we use the group winners and runners-up
							PlayerId playerA = (PlayerId)$"BYE";
							PlayerId playerB = (PlayerId)$"BYE";
							if (drawPlacement < noOfGroups) {
								playerA = (PlayerId)$"|Group {(char)(winnerPositions[drawPlacement] + 'A')} Winner";
								playerB = (PlayerId)$"|Group {(char)(runnerUpPositions[drawPlacement] + 'A')} Runner Up";
							}
							matches.Add(new Match(
								(MatchId)$"{roundName} {matchNo}",
								playerA,
								playerB,
								0,
								0,
								null,
								null));
						} else {
							matches.Add(new Match(
								(MatchId)$"{roundName} {matchNo}",
								(PlayerId)$"|{matchNo - (matchesPerRound * 2) + i}",
								(PlayerId)$"|{matchNo - (matchesPerRound * 2) + i + 1}",
								0,
								0,
								null,
								null));
						}

					}

					knockoutRounds.Add(new KnockoutRound($"{roundName}", [.. matches]));
				}

				knockoutStage = new KnockoutStage("Knockout", [], knockoutRounds);



				newTournament = tournament with { KnockoutStage = knockoutStage };
			}

			//if (tournament.GroupsCompleted is false)
			//{
			//	message = "Cannot draw knockout stage for a tournament with incomplete groups.";
			//	return false;
			//}

			// take top 2 players from each group
			foreach (Group group in newTournament.Groups)
			{
				if (group.IsCompleted)
				{
					int winnerIndex = newTournament.KnockoutStage!
						.Rounds[0]
						.Matches
						.FindIndex(m => m.PlayerA.IsPlaceHolder && m.PlayerA.stringId == $"|{group.Name} Winner" );
					if (winnerIndex >= 0) {
						newTournament.KnockoutStage!.Rounds[0].Matches[winnerIndex]
							= newTournament.KnockoutStage!.Rounds[0].Matches[winnerIndex] with { PlayerA = group.GroupPositions[0].PlayerId };
					}

					int runnerUpIndex = newTournament.KnockoutStage!
						.Rounds[0]
						.Matches
						.FindIndex(m => m.PlayerB.IsPlaceHolder && m.PlayerB.stringId == $"|{group.Name} Runner Up" );
					if (runnerUpIndex >= 0) {
						newTournament.KnockoutStage!.Rounds[0].Matches[runnerUpIndex]
							= newTournament.KnockoutStage!.Rounds[0].Matches[runnerUpIndex] with { PlayerB = group.GroupPositions[1].PlayerId };
					}
				}
			}

			for (int matchIdx = 0; matchIdx < newTournament.KnockoutStage!.Rounds[0].Matches.Count; matchIdx++)
			{
				Match match = newTournament.KnockoutStage!.Rounds[0].Matches[matchIdx];
				if (match.PlayerA.IsPlayer && match.PlayerB.IsPlayer)
				{
					newTournament.KnockoutStage!.Rounds[0].Matches[matchIdx] = match with
					{
						PlayerAStart = newTournament.GetPlayer(match.PlayerA).StartingHandicap(newTournament.GetPlayer(match.PlayerB)),
						PlayerBStart = newTournament.GetPlayer(match.PlayerB).StartingHandicap(newTournament.GetPlayer(match.PlayerA))
					};
				}
			}

			return true;
		}

	}
}