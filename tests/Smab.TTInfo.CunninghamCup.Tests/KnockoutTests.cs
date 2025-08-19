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
		List<Group> groups = [.. tournament.Groups];
		for (int groupIdx = 0; groupIdx < groups.Count; groupIdx++) {
			Group group = groups[groupIdx];
			groups[groupIdx] = group.CompleteWithRandomResults();
			testOutputHelper.WriteLine(group.AsString(tournament));
			
		}

		return tournament with { Groups = groups };
	}

	private Tournament RunGroup(Tournament tournament, int groupIndex)
	{
		List<Group> groups = [.. tournament.Groups];
		Group group = groups[groupIndex];
		groups[groupIndex] = group.CompleteWithRandomResults();

		return tournament with { Groups = groups };
	}



	[Theory]
	[InlineData(32, 4, 4)]
	[InlineData(28, 4, 4)]
	[InlineData(20, 5, 3)]
	public void Tournament_Should_Run(int noOfPlayers, int groupSize, int expectedNoOfRounds)
	{
		Tournament? tournament = CreateTestTournamentWithPlayers(noOfPlayers, groupSize);

		// Ensure the tournament has groups drawn
		tournament.TryDrawKnockoutStage(out tournament, out string? message).ShouldBeTrue();
		tournament.KnockoutStage!.Rounds.Count.ShouldBe(expectedNoOfRounds);
		tournament.KnockoutStage!.Rounds[0].Matches
			.Any(match => match.PlayerA.IsPlayer).ShouldBeFalse();
		message.ShouldBeEmpty();

		// Run a single group to completion
		tournament = RunGroup(tournament, 2);
		tournament.GroupsCompleted.ShouldBeFalse();
		tournament.Groups[2].IsCompleted.ShouldBeTrue();

		// Ensure the tournament can still draw knockout stage
		tournament.TryDrawKnockoutStage(out tournament, out message).ShouldBeTrue();
		tournament.KnockoutStage!.Rounds.Count.ShouldBe(expectedNoOfRounds);
		tournament.KnockoutStage!.Rounds[0].Matches
			.Any(match => match.PlayerA.IsPlayer).ShouldBeTrue();
		message.ShouldBeEmpty();

		// Run all groups to completion
		tournament = RunTheGroups(tournament);
		tournament.GroupsCompleted.ShouldBeTrue();
		bool success = tournament.TryDrawKnockoutStage(out tournament, out message);
		success.ShouldBeTrue();
		message.ShouldBeEmpty();
		_ = tournament.ShouldNotBeNull();
		_ = tournament!.KnockoutStage.ShouldNotBeNull();
		tournament.KnockoutStage!.Rounds.Count.ShouldBe(expectedNoOfRounds);

	}
}