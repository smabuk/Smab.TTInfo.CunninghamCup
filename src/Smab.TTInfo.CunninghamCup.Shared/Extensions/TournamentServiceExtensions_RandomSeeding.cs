namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static partial class TournamentServiceExtensions
{
	extension(ITournamentService tournamentService)
	{
		public ITournamentService SeedRandomTournament(int noOfPlayers)
		{
			Tournament tournament = Tournament.Create(
				name: "Test Cunningham Cup",
				date: DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
				players: [.. Enumerable.Range(1, noOfPlayers)
				.Select(i => Player.Create($"Player {i}", Random.Shared.Next(-10, 10)))]
			);

			tournamentService.AddOrUpdateTournament(tournament);

			return tournamentService;
		}

		public Group CompleteGroupWithRandomResults(string groupName)
		{
			Tournament tournament = tournamentService.GetTournament();
			int groupIndex = tournament.Groups.FindIndex(g => g.Name == groupName);
			Group group = tournament.Groups[groupIndex];

			List<Match> matches = [.. group.Matches];
			for (int i = 0; i < matches.Count; i++) {
				if (!matches[i].IsCompleted) {
					matches[i] = SetRandomResult(matches[i]);
				}
			}

			tournament.Groups[groupIndex] = group with { Matches = [.. matches] };
			tournamentService.AddOrUpdateTournament(tournament);

			return tournament.Groups[groupIndex];
		}

		public Group CompleteMatchWithRandomScores(string groupName, int matchNo)
		{
			Tournament tournament = tournamentService.GetTournament();
			int groupIndex = tournament.Groups.FindIndex(g => g.Name == groupName);
			Group group = tournament.Groups[groupIndex];

			group.Matches[matchNo] = SetRandomResult(group.Matches[matchNo]);

			tournament.Groups[groupIndex] = group;
			tournamentService.AddOrUpdateTournament(tournament);

			return tournament.Groups[groupIndex];
		}

		public Group ResetGroup(string groupName)
		{
			Tournament tournament = tournamentService.GetTournament();
			int groupIndex = tournament.Groups.FindIndex(g => g.Name == groupName);
			Group group = tournament.Groups[groupIndex];

			List<Match> matches = [.. group.Matches];
			for (int i = 0; i < matches.Count; i++) {
				matches[i] = matches[i] with { Result = null };
			}

			tournament.Groups[groupIndex] = group with { Matches = [.. matches] };
			tournamentService.AddOrUpdateTournament(tournament);

			return tournament.Groups[groupIndex];
		}
	}

	public static Match SetRandomResult(Match match)
	{
		bool[] playerAWins = [
			Random.Shared.Next(2) == 0,
				Random.Shared.Next(2) == 0,
				Random.Shared.Next(2) == 0];

		List<Set> sets = playerAWins switch
		{
			[true, true, _] => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(21, Random.Shared.Next(match.PlayerBStart, 20))],
			[false, false, _] => [new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21)],
			[_, _, true] => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(21, Random.Shared.Next(match.PlayerBStart, 20))],
			_ => [new Set(21, Random.Shared.Next(match.PlayerBStart, 20)), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21), new Set(Random.Shared.Next(match.PlayerAStart, 20), 21)],
		};

		return match.SetResult(sets);
	}
}
