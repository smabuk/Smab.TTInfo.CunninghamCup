namespace Smab.TTInfo.CunninghamCup.Tests;

public class TournamentTests
{
	[Fact]
	public void Create_Tournament_With_Name_And_Date_Should_Initialize_Empty_Groups_And_Players()
	{
		string name = "Test Tournament";
		DateTime date = new(2025, 8, 17);

		Tournament tournament = TournamentExtensions.Create(name, date);

		tournament.Name.ShouldBe(name);
		tournament.Date.ShouldBe(date);
		tournament.Groups.ShouldBeEmpty();
		tournament.Players.ShouldBeEmpty();
	}

	[Fact]
	public void Create_Tournament_With_Players_Should_Initialize_Players()
	{
		string name = "Test Tournament";
		DateTime date = new(2025, 8, 17);
		List<PlayerEntry> players =
		[
			new(new Player("Alice"), 10),
			new(new Player("Bob"), 20)
		];

		Tournament tournament = TournamentExtensions.Create(name, date, players);

		tournament.Players.Count.ShouldBe(2);
		tournament.Players[0].Player.Name.ShouldBe("Alice");
		tournament.Players[1].Player.Name.ShouldBe("Bob");
	}

	[Fact]
	public void GroupsCount_Should_Return_Number_Of_Groups()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);
		tournament.Groups.Add(new Group("A", [], []));
		tournament.Groups.Add(new Group("B", [], []));

		tournament.GroupsCount.ShouldBe(2);
	}

	[Fact]
	public void PlayersCount_Should_Return_Number_Of_Players()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);
		tournament.Players.Add(new PlayerEntry(new Player("Alice"), 10));
		tournament.Players.Add(new PlayerEntry(new Player("Bob"), 20));

		tournament.PlayersCount.ShouldBe(2);
	}

	[Fact]
	public void HasGroups_Should_Return_True_If_Groups_Exist()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);
		tournament.Groups.Add(new Group("A", [], []));

		tournament.HasGroups.ShouldBeTrue();
	}

	[Fact]
	public void HasGroups_Should_Return_False_If_No_Groups()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);

		tournament.HasGroups.ShouldBeFalse();
	}

	[Fact]
	public void AddOrUpdatePlayer_Should_Add_New_Player_If_Not_Exists()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);

		tournament.AddOrUpdatePlayer("Alice", 15);

		tournament.Players.Count.ShouldBe(1);
		tournament.Players[0].Player.Name.ShouldBe("Alice");
		tournament.Players[0].Handicap.ShouldBe(15);
	}

	[Fact]
	public void AddOrUpdatePlayer_Should_Update_Handicap_If_Player_Exists()
	{
		Tournament tournament = TournamentExtensions.Create("Test", DateTime.Now);
		tournament.AddOrUpdatePlayer("Alice", 10);

		tournament.AddOrUpdatePlayer("Alice", 20);

		tournament.Players.Count.ShouldBe(1);
		tournament.Players[0].Handicap.ShouldBe(20);
	}
}