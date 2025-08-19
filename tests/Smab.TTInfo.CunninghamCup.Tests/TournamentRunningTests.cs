namespace Smab.TTInfo.CunninghamCup.Tests;

public class TournamentRunningTests(ITestOutputHelper testOutputHelper)
{
	private Tournament CreateTestTournamentWithPlayers(int playerCount)
	{
		List<Player> players = [];
		for (int i = 0; i < playerCount; i++) {
			Player player = Player.Create(($"Player {i + 1}"), Random.Shared.Next(-10, 10));
			players.Add(player);
		}

		return Tournament.Create("Test Tournament", DateOnly.FromDateTime(DateTime.Now), players);
	}

	[Theory]
	[InlineData(3, 10)]
	[InlineData(4, 21)]
	public void DrawGroups_Should_Distribute_Players_Into_Correct_Number_Of_Groups(int groupSize, int noOfPlayers)
	{
		Tournament tournament = CreateTestTournamentWithPlayers(noOfPlayers);
		int expectedGroupCount = (int)Math.Ceiling((double)noOfPlayers / groupSize);

		tournament = tournament.DrawGroups(groupSize);

		tournament.Groups.Count.ShouldBe(expectedGroupCount);

		int totalPlayers = tournament.Groups.Sum(g => g.Players.Count);
		totalPlayers.ShouldBe(noOfPlayers);
	}

	[Fact]
	public void DrawGroups_Should_Throw_If_Groups_Already_Exist()
	{
		Tournament tournament = CreateTestTournamentWithPlayers(5);
		tournament.Groups.Add(new Group("Group1", [], []));

		_ = Should.Throw<InvalidOperationException>(() => tournament.DrawGroups(2));
	}

	[Theory]
	[InlineData(3, 10)]
	[InlineData(4, 21)]
	[InlineData(4, 32)]
	[InlineData(5, 38)]
	public void DrawGroups_Should_Distribute_Players_Evenly(int groupSize, int noOfPlayers)
	{
		Tournament tournament = CreateTestTournamentWithPlayers(noOfPlayers);
		int expectedGroupCount = (int)Math.Ceiling((double)noOfPlayers / groupSize);

		tournament = tournament.DrawGroups(groupSize);
		tournament.Groups.Count.ShouldBe(expectedGroupCount);

		// Groups should have either n or n-1 players
		foreach (Group group in tournament.Groups) {
			testOutputHelper.WriteLine(group.AsString(tournament));
			group.Players.Count.ShouldBeInRange(groupSize - 1, groupSize);
		}
	}

	[Fact]
	public void Run_Group_Stage()
	{
		int groupSize = 4;
		Tournament tournament = CreateTestTournamentWithPlayers(groupSize);

		tournament = tournament.DrawGroups(groupSize);

		// Groups should have either n or n-1 players
		foreach (Group group in tournament.Groups) {
			testOutputHelper.WriteLine(group.AsString(tournament));
			group.Players.Count.ShouldBeInRange(groupSize - 1, groupSize);
		}


	}
}