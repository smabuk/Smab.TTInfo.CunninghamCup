using System.Diagnostics.CodeAnalysis;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static partial class TournamentExtensions
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

			// Distribute players into groups
			List<List<Player>> playerGroupings = [.. Enumerable
				.Range(0, groupCount)
				.Select(groupIdx => shuffledPlayers
					.Where((_, playerIdx) => playerIdx % groupCount == groupIdx)
					.ToList())];

			List<Group> groups = [.. Enumerable
				.Range(0, groupCount)
				.Select(i => Group.Create($"Group {(char)(i + 'A')}",  [.. playerGroupings[i]]))
				];

			for (int i = 0; i < groups.Count; i++)
			{
				Group group = groups[i];
				groups[i] = i < 2
					? (group with { ScheduledTime = tournament.Date.ToDateTime(new TimeOnly(09, 30)) })
					: (group with { ScheduledTime = tournament.Date.ToDateTime(new TimeOnly(11, 00)) });
			}

			return tournament with { Groups = groups };
		}

		public static List<Match> CreateFirstKnockoutRound(
			int matchesPerRound,
			int noOfGroups,
			RoundType roundType,
			List<int> positions
		)
		{
			int noOfByes = (matchesPerRound * 2) - (noOfGroups * 2);
			// Assuming 2 players from each group qualify for the knockout stage
			List<int> winnerPositions = [.. Enumerable.Range(0, matchesPerRound).Select(i => i * 2).Shuffle().Take(noOfGroups)];
			List<int> byePositions = [.. winnerPositions.Take(noOfByes).Select(i => i + 1)];
			if (byePositions.Count < noOfByes) {
				byePositions = [
					.. byePositions,
					.. Enumerable.Range(0, matchesPerRound * 2)
						.Except(winnerPositions)
						.Except(byePositions)
						.Shuffle()
						.Take(noOfByes - byePositions.Count)
						];
			}

			List<int> runnerUpPositions = [.. Enumerable.Range(0, matchesPerRound * 2).Except(winnerPositions).Except(byePositions).Shuffle()];

			while (winnerPositions.Zip(runnerUpPositions).Any(p => p.First + 1 == p.Second)) {
				runnerUpPositions = [.. runnerUpPositions.Shuffle()];
			}

			PlayerId[] firstRoundPlacements = new PlayerId[matchesPerRound * 2];
			for (int i = 0; i < winnerPositions.Count; i++) {
				firstRoundPlacements[winnerPositions[i]] = (PlayerId)$"{PlayerId.PlaceHolderSymbol}Group {(char)(i + 'A')} {PositionName(positions[0])}";
			}

			for (int i = 0; i < runnerUpPositions.Count; i++) {
				firstRoundPlacements[runnerUpPositions[i]] = (PlayerId)$"{PlayerId.PlaceHolderSymbol}Group {(char)(i + 'A')} {PositionName(positions[1])}";
			}

			for (int i = 0; i < byePositions.Count; i++) {
				firstRoundPlacements[byePositions[i]] = PlayerId.Bye;
			}

			List<Match> matches = [];
			for (int i = 0; i < matchesPerRound * 2; i += 2) {
				PlayerId playerA = firstRoundPlacements[i];
				PlayerId playerB = firstRoundPlacements[i+ 1];

				matches.Add(new Match(
					(MatchId)$"{roundType} {(i / 2) + 1:D2}",
					playerA,
					playerB,
					0,
					0,
					null,
					null));
			}

			return matches;
		}

		public KnockoutStage DrawKnockoutStage(string name, List<int> positions, bool redraw = false)
		{
			//if (knockoutStage is not null && redraw is false) {
			//	return knockoutStage;
			//}

			int noOfRounds = (tournament.GroupsCount, tournament.ActivePlayers.Count) switch
			{
				(4, _) => 3,
				(> 4 and <= 8, _) => 4,
				(_, > 23) => 4,
				(_, > 12) => 3,
				(_, > 8) => 2,
				_ => 0
			};

			List<KnockoutRound> knockoutRounds = [];
			int matchNo = 0;
			for (int round = 0; round < noOfRounds; round++) {
				int matchesPerRound = (int)Math.Pow(2, noOfRounds - round) / 2;
				List<Match> matches = [];
				RoundType roundType = (RoundType)(noOfRounds - round);

				int noOfGroups = tournament.GroupsCount;
				if (round == 0) {
					matches = CreateFirstKnockoutRound(matchesPerRound, noOfGroups, roundType, positions);
					matchNo += matches.Count;
				} else {
					for (int i = 0; i < matchesPerRound; i++) {
						matchNo++;
						matches.Add(new Match(
							(MatchId)$"{roundType} {matchNo:D2}",
							(PlayerId)$"{PlayerId.PlaceHolderSymbol}{matchNo - (matchesPerRound * 2) + i:D2}",
							(PlayerId)$"{PlayerId.PlaceHolderSymbol}{matchNo - (matchesPerRound * 2) + i + 1:D2}",
							0,
							0,
							null,
							null));
					}
				}

				knockoutRounds.Add(new KnockoutRound(roundType, [.. matches]));
			}

			return new KnockoutStage(name, positions, knockoutRounds);
		}


		public bool TryDrawKnockoutStage(KnockoutStage knockoutStage, out Tournament newTournament, [NotNullWhen(false)] out string? message)
		{
			newTournament = tournament;
			message = "";

			if (knockoutStage is null) { 
				message = "Cannot draw knockout stage for a tournament with no knockout stage defined.";
				return false;
			}

			int winnersPosition = knockoutStage.GroupPositions[0];
			int runnersUpPosition = knockoutStage.GroupPositions[1];
			string winnersName   = PositionName(winnersPosition);
			string runnersUpName = PositionName(runnersUpPosition);

			for (int roundIdx = 0; roundIdx < knockoutStage.Rounds.Count; roundIdx++) {
				KnockoutRound knockoutRound = knockoutStage.Rounds[roundIdx];
				if (roundIdx is 0 && knockoutRound.IsNotPopulated)
				{
					// take 2 players from each group
					foreach (Group group in newTournament.Groups) {
						if (group.IsCompleted) {
							Match match = null!;
							int winnerIndex = knockoutRound
								.Matches
								.FindIndex(m => m.PlayerA.IsPlaceHolder && m.PlayerA.StringId == $"{PlayerId.PlaceHolderSymbol}{group.Name} {winnersName}");
							if (winnerIndex >= 0) {
								match = knockoutRound.Matches[winnerIndex];
								match = match with { PlayerA = group.GroupPositions[winnersPosition].PlayerId };
								if (match.PlayerB.IsPlayer) {
									(int playerAStart, int playerBStart) = newTournament.StartingHandicap(match.PlayerA, match.PlayerB);
									match = match with
									{
										PlayerAStart = playerAStart,
										PlayerBStart = playerBStart,
									};
								}

								knockoutRound.Matches[winnerIndex] = match;
							}

							int runnerUpIndex = knockoutRound
								.Matches
								.FindIndex(m => m.PlayerB.IsPlaceHolder && m.PlayerB.StringId == $"{PlayerId.PlaceHolderSymbol}{group.Name} {runnersUpName}");
							if (runnerUpIndex < 0) {
								runnerUpIndex = knockoutRound
									.Matches
									.FindIndex(m => m.PlayerA.IsPlaceHolder && m.PlayerA.StringId == $"{PlayerId.PlaceHolderSymbol}{group.Name} {runnersUpName}");
								if (runnerUpIndex >= 0) {
									match = knockoutRound.Matches[runnerUpIndex];
									match = match with { PlayerA = group.GroupPositions[runnersUpPosition].PlayerId };
								}
							} else {
								match = knockoutRound.Matches[runnerUpIndex];
								match = match with { PlayerB = group.GroupPositions[runnersUpPosition].PlayerId };
							}

							if (runnerUpIndex >= 0) {
								if (match.PlayerA.IsPlayer && match.PlayerB.IsPlayer) {
									(int playerAStart, int playerBStart) = newTournament.StartingHandicap(match.PlayerA, match.PlayerB);
									match = match with
									{
										PlayerAStart = playerAStart,
										PlayerBStart = playerBStart,
									};

								}

								knockoutRound.Matches[runnerUpIndex] = match;
							}
						}
					}
				}

				if (roundIdx > 0)
				{
					KnockoutRound previousRound = knockoutStage.Rounds[roundIdx - 1];
					for (int matchIdx = 0; matchIdx < knockoutRound.Matches.Count; matchIdx++) {
						Match match = knockoutRound.Matches[matchIdx];
						if (match.PlayerA.IsPlaceHolder) {
							Match? previousMatch = previousRound.Matches
								.SingleOrDefault(m => m.Id.StringId.EndsWith(match.PlayerA.StringId[1..]));
							if (previousMatch is not null && previousMatch.IsCompleted) {
								match = match with { PlayerA = previousMatch.Winner ?? match.PlayerA };
							}

							if (match.PlayerA.IsPlayer && match.PlayerB.IsPlayer) {
								(int playerAStart, int playerBStart) = newTournament.StartingHandicap(match.PlayerA, match.PlayerB);
								match = match with
								{
									PlayerAStart = playerAStart,
									PlayerBStart = playerBStart,
								};

							}

						}

						if (match.PlayerB.IsPlaceHolder) {
							Match? previousMatch = previousRound.Matches
								.SingleOrDefault(m => m.Id.StringId.EndsWith(match.PlayerB.StringId[1..]));
							if (previousMatch is not null && previousMatch.IsCompleted) {
								match = match with { PlayerB = previousMatch.Winner ?? match.PlayerB };
							}

							if (match.PlayerA.IsPlayer && match.PlayerB.IsPlayer) {
								(int playerAStart, int playerBStart) = newTournament.StartingHandicap(match.PlayerA, match.PlayerB);
								match = match with
								{
									PlayerAStart = playerAStart,
									PlayerBStart = playerBStart,
								};

							}

						}

						knockoutRound.Matches[matchIdx] = match;

					}
				}

			}

			return true;
		}

		private static string PositionName(int position)
		{
			return position switch
			{
				0 => "Winner",
				1 => "Runner Up",
				2 => "Third Place",
				3 => "Fourth Place",
				4 => "Fifth Place",
				5 => "Sixth Place",
				_ => "Unknown"
			};
		}
	}
}