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
			groups[groupIdx] = groups[groupIdx].CompleteWithRandomResults();
			testOutputHelper.WriteLine(groups[groupIdx].AsString(tournament));
			
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
	[InlineData(15, 3, 4)]
	[InlineData(15, 4, 3)]
	[InlineData(32, 4, 4)]
	[InlineData(28, 4, 4)]
	[InlineData(20, 5, 3)]
	public void Tournament_Should_Run(int noOfPlayers, int groupSize, int expectedNoOfRounds)
	{
		Tournament? tournament = CreateTestTournamentWithPlayers(noOfPlayers, groupSize);

		// Ensure the tournament has groups drawn
		tournament = tournament with { KnockoutStage = tournament.DrawKnockoutStage("Knockout Stage", [0, 1])};
		tournament.KnockoutStage!.Rounds.Count.ShouldBe(expectedNoOfRounds);
		tournament.KnockoutStage.Rounds[0].Matches
			.Any(match => match.PlayerA.IsPlayer).ShouldBeFalse();

		// Run a single group to completion
		tournament = RunGroup(tournament, 2);
		tournament.GroupsCompleted.ShouldBeFalse();
		tournament.Groups[2].IsCompleted.ShouldBeTrue();

		// Ensure the tournament can still draw knockout stage
		tournament.TryDrawKnockoutStage(tournament.KnockoutStage!, out tournament, out string? message).ShouldBeTrue();
		if(tournament.KnockoutStage is null)
		{
			return;
		}

		tournament.KnockoutStage.Rounds.Count.ShouldBe(expectedNoOfRounds);
		tournament.KnockoutStage.Rounds[0].Matches
			.Any(match => match.PlayerA.IsPlayer).ShouldBeTrue();
		tournament.KnockoutStage.Rounds[0].IsPopulated.ShouldBeFalse();
		tournament.KnockoutStage.Rounds[0].IsCompleted.ShouldBeFalse();
		message.ShouldBeEmpty();

		// Run all groups to completion
		tournament = RunTheGroups(tournament);
		tournament.GroupsCompleted.ShouldBeTrue();

		testOutputHelper.WriteLine("After 1 group has been added to the knockout stage:");
		testOutputHelper.WriteLine(tournament.KnockoutStage!.Rounds[0].AsString());
		testOutputHelper.WriteLine(tournament.KnockoutStage.Rounds[1].AsString());


		bool success = tournament.TryDrawKnockoutStage(tournament.KnockoutStage, out tournament, out message);
		success.ShouldBeTrue();
		message.ShouldBeEmpty();
		_ = tournament.ShouldNotBeNull();
		_ = tournament!.KnockoutStage.ShouldNotBeNull();
		tournament.KnockoutStage.Rounds.Count.ShouldBe(expectedNoOfRounds);
		tournament.KnockoutStage.Rounds[0].IsPopulated.ShouldBeTrue();
		tournament.KnockoutStage.Rounds[0].IsCompleted.ShouldBeFalse();

		for (int roundIndex = 0; roundIndex < tournament.KnockoutStage!.Rounds.Count; roundIndex++) {

			KnockoutRound round = tournament.KnockoutStage.Rounds[roundIndex];
			for (int matchIndex = 0; matchIndex < round.Matches.Count; matchIndex++) {
				Match match = round.Matches[matchIndex];
				if (match.IsCompleted) {
					continue;
				}

				match = match.SetRandomResult();
				round.Matches[matchIndex] = match;
			}

			round.IsCompleted.ShouldBeTrue();
			bool _ = tournament.TryDrawKnockoutStage(tournament.KnockoutStage, out tournament, out message);
		}

		testOutputHelper.WriteLine("After all the rounds have been played:");
		foreach (KnockoutRound round in tournament.KnockoutStage!.Rounds) {
			testOutputHelper.WriteLine(round.AsString());
			round.IsCompleted.ShouldBeTrue();
			round.IsPopulated.ShouldBeTrue();
			round.Matches
				.SelectMany(m => (List<PlayerId>)[m.PlayerA, m.PlayerB])
				.Where(p => p.IsPlayer)
				.ShouldBeUnique();
			round.Matches.All(m => m.IsCompleted).ShouldBeTrue();
		}

	}
}