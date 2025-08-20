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
				.Select(i => Group.Create($"Group {(char)(i + 'A')}", [.. playerGroupings[i]]))
				];

			return tournament with { Groups = groups };
		}

		public static List<Match> CreateFirstKnockoutRound(
			int matchesPerRound,
			int noOfGroups,
			RoundType roundType
		)
		{

			List<int> winnerPositions = [.. Enumerable.Range(0, noOfGroups).Shuffle()];
			List<int> runnerUpPositions = [.. Enumerable.Range(0, noOfGroups).Shuffle()];

			while (winnerPositions.Zip(runnerUpPositions).Any(p => p.First == p.Second)) {
				runnerUpPositions = [.. runnerUpPositions.Shuffle()];
			}

			List<int> firstRoundPlacements = [.. Enumerable.Range(0, matchesPerRound).Shuffle()];

			List<Match> matches = [];
			for (int i = 0; i < matchesPerRound; i++) {
				PlayerId playerA = PlayerId.Bye;
				PlayerId playerB = PlayerId.Bye;
				int drawPlacement = firstRoundPlacements[i];
				if (drawPlacement < noOfGroups) {
					playerA = (PlayerId)$"{PlayerId.PlaceHolderSymbol}Group {(char)(winnerPositions[drawPlacement] + 'A')} Winner";
					playerB = (PlayerId)$"{PlayerId.PlaceHolderSymbol}Group {(char)(runnerUpPositions[drawPlacement] + 'A')} Runner Up";
				}

				matches.Add(new Match(
					(MatchId)$"{roundType} {(i + 1):D2}",
					playerA,
					playerB,
					0,
					0,
					null,
					null));
			}

			return matches;
		}

		public KnockoutStage DrawKnockoutStage(string name, bool redraw = false)
		{
			if (tournament.KnockoutStage is not null && redraw is false) {
				return tournament.KnockoutStage;
			}

			int noOfRounds = (tournament.GroupsCount, tournament.ActivePlayers.Count) switch
			{
				(1, _) => 0,
				(_, > 23) => 4,
				(_, > 16) => 3,
				(_, > 8) => 2,
				(8, _) => 4,
				_ => 0
			};

			List<KnockoutRound> knockoutRounds = [];
			int matchNo = 0;
			for (int round = 0; round < noOfRounds; round++) {
				int matchesPerRound = (int)Math.Pow(2, noOfRounds - round) / 2;
				List<Match> matches = [];
				RoundType roundType = (RoundType)(noOfRounds - round);
				//string roundName = roundType.ToString();

				int noOfGroups = tournament.GroupsCount;
				if (round == 0) {
					matches = CreateFirstKnockoutRound(matchesPerRound, noOfGroups, roundType);
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

			return new KnockoutStage(name, knockoutRounds);
		}


		public bool TryDrawKnockoutStage(out Tournament newTournament, [NotNullWhen(false)] out string? message)
		{
			newTournament = tournament;
			message = "";

			//if (newTournament.KnockoutStage is null)
			//{
			//	newTournament = tournament with { KnockoutStage = tournament.DrawKnockoutStage("Knockout Stage") };
			//}

			if (newTournament.KnockoutStage is null) { 
				message = "Cannot draw knockout stage for a tournament with no knockout stage defined.";
				return false;
			}

			for (int roundIdx = 0; roundIdx < newTournament.KnockoutStage.Rounds.Count; roundIdx++) {
				KnockoutRound knockoutRound = newTournament.KnockoutStage.Rounds[roundIdx];
				if (roundIdx is 0 && knockoutRound.IsNotPopulated)
				{
					// take top 2 players from each group
					foreach (Group group in newTournament.Groups) {
						if (group.IsCompleted) {
							int winnerIndex = knockoutRound
								.Matches
								.FindIndex(m => m.PlayerA.IsPlaceHolder && m.PlayerA.StringId == $"{PlayerId.PlaceHolderSymbol}{group.Name} Winner");
							if (winnerIndex >= 0) {
								Match match = knockoutRound.Matches[winnerIndex];
								match = match with { PlayerA = group.GroupPositions[0].PlayerId };
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
								.FindIndex(m => m.PlayerB.IsPlaceHolder && m.PlayerB.StringId == $"{PlayerId.PlaceHolderSymbol}{group.Name} Runner Up");
							if (runnerUpIndex >= 0) {
								Match match = knockoutRound.Matches[runnerUpIndex];
								match = match with { PlayerB = group.GroupPositions[1].PlayerId };
								if (match.PlayerA.IsPlayer) {
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
					KnockoutRound previousRound = newTournament.KnockoutStage.Rounds[roundIdx - 1];
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

	}
}