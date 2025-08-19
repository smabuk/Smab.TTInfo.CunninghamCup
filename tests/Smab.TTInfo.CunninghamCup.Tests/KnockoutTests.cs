namespace Smab.TTInfo.CunninghamCup.Tests;

public class KnockoutTests(ITestOutputHelper testOutputHelper)
{
	private static Tournament CreateTestTournamentWithPlayers(int playerCount, int groupSize)
	{
		string name = "Test Tournament";
		DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

		List<Player> players = [..
			Enumerable.Range(0, playerCount)
			.Select(i => Player.Create($"Player {i}", i * 2))];

		Tournament tournament = Tournament.Create(name, tomorrow, players);
		tournament = tournament.DrawGroups(groupSize);

		return tournament;
	}

	private Tournament RunTheGroups(Tournament tournament)
	{
		foreach (Group group in tournament.Groups) {
			for (int i = 0; i < group.Matches.Count; i++) {
				bool[] playerAWins = [Random.Shared.Next(2) == 0, Random.Shared.Next(2) == 0, Random.Shared.Next(2) == 0];
				List<Set> sets = playerAWins switch
				{
					[true, true, _] => [new Set(21, Random.Shared.Next(19)), new Set(21, Random.Shared.Next(19))],
					[false, false, _] => [new Set(Random.Shared.Next(19), 21), new Set(Random.Shared.Next(19), 21)],
					[_, _, true] => [new Set(21, Random.Shared.Next(19)), new Set(Random.Shared.Next(19), 21), new Set(21, Random.Shared.Next(19))],
					_ => [new Set(21, Random.Shared.Next(19)), new Set(Random.Shared.Next(19), 21), new Set(Random.Shared.Next(19), 21)],
				};
				group.Matches[i] = group.Matches[i].SetResult(sets);
			}

			testOutputHelper.WriteLine(group.AsString(tournament));
		}

		return tournament;
	}



	[Fact]
	public void Tournament_Should_Run()
	{
		int noOfPlayers = 32;
		int groupSize = 4;
		Tournament tournament = CreateTestTournamentWithPlayers(noOfPlayers, groupSize);
		tournament = RunTheGroups(tournament);
		tournament.GroupsCompleted.ShouldBeTrue();
		bool success = tournament.TryDrawKnockoutStage(out Tournament? newTournament, out string? message);
		success.ShouldBeTrue();
		message.ShouldBeEmpty();
		_ = newTournament.ShouldNotBeNull();
		_ = newTournament!.KnockoutStage.ShouldNotBeNull();
		newTournament.KnockoutStage!.Rounds.Count.ShouldBe(4); // 16 players qualiify -> 4 rounds
	}
}