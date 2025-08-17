namespace Smab.TTInfo.CunninghamCup.Tests;

public class TournamentRunningExtensionsTests
{
	private Tournament CreateTournamentWithPlayers(int playerCount)
	{
		List<Player> players = [];
		for (int i = 0; i < playerCount; i++) {
			Player player = Player.Create(($"Player {i + 1}"), Random.Shared.Next(-10, 10));
			players.Add(player);
		}

		return Tournament.Create("Test Tournament", DateTime.Now, players);
	}

	[Fact]
	public void DrawGroups_Should_Distribute_Players_Into_Correct_Number_Of_Groups()
	{
		Tournament tournament = CreateTournamentWithPlayers(10);
		int groupSize = 3;
		int expectedGroupCount = (int)Math.Ceiling(10.0 / groupSize);

		tournament = tournament.DrawGroups(groupSize);

		tournament.Groups.Count.ShouldBe(expectedGroupCount);
		int totalPlayers = 0;
		foreach (Group group in tournament.Groups) {
			totalPlayers += group.Players.Count;
		}

		totalPlayers.ShouldBe(10);
	}

	[Fact]
	public void DrawGroups_Should_Throw_If_Groups_Already_Exist()
	{
		Tournament tournament = CreateTournamentWithPlayers(5);
		tournament.Groups.Add(new Group("Group1", [], []));

		_ = Should.Throw<InvalidOperationException>(() => tournament.DrawGroups(2));
	}

	[Fact]
	public void DrawGroups_Should_Distribute_Players_Evenly()
	{
		Tournament tournament = CreateTournamentWithPlayers(21);
		int groupSize = 4;
		int expectedGroupCount = (int)Math.Ceiling(21.0 / groupSize);

		tournament = tournament.DrawGroups(groupSize);
		tournament.Groups.Count.ShouldBe(expectedGroupCount);

		// Groups should have either 3 or 4 players
		foreach (Group group in tournament.Groups) {
			group.Players.Count.ShouldBeInRange(3, 4);
		}
	}
}