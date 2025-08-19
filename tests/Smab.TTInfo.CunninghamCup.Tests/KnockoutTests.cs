namespace Smab.TTInfo.CunninghamCup.Tests;

public class KnockoutTests(ITestOutputHelper testOutputHelper)
{
	private static Tournament CreateTestTournamentWithPlayers(int playerCount, int groupSize)
	{
		string name = "Test Tournament";
		DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

		List<Player> players = [..
			Enumerable.Range(0, playerCount)
			.Select(i => Player.Create($"Player {i}", Random.Shared.Next(-10, 10)))];

		Tournament tournament = Tournament.Create(name, tomorrow, players);
		tournament = tournament.DrawGroups(groupSize);

		return tournament;
	}

	private Tournament RunTheGroups(Tournament tournament)
	{
		foreach (Group group in tournament.Groups) {
			for (int i = 0; i < group.Matches.Count; i++) {
				group.Matches[i] = group.Matches[i].SetRandomResult();
			}

			testOutputHelper.WriteLine(group.AsString(tournament));
		}

		return tournament;
	}



	[Theory]
	[InlineData(32, 4, 4)]
	[InlineData(28, 4, 4)]
	[InlineData(20, 5, 3)]
	public void Tournament_Should_Run(int noOfPlayers, int groupSize, int expectedNoOfRounds)
	{
		Tournament tournament = CreateTestTournamentWithPlayers(noOfPlayers, groupSize);
		tournament = RunTheGroups(tournament);
		tournament.GroupsCompleted.ShouldBeTrue();
		bool success = tournament.TryDrawKnockoutStage(out Tournament? newTournament, out string? message);
		success.ShouldBeTrue();
		message.ShouldBeEmpty();
		_ = newTournament.ShouldNotBeNull();
		_ = newTournament!.KnockoutStage.ShouldNotBeNull();
		newTournament.KnockoutStage!.Rounds.Count.ShouldBe(expectedNoOfRounds);
	}
}