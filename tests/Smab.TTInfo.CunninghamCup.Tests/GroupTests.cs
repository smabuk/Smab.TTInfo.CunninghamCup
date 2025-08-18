namespace Smab.TTInfo.CunninghamCup.Tests;

public class GroupTests(ITestOutputHelper testOutputHelper)
{
	/// <summary>
	/// Creates a new group with the specified number of players.
	/// </summary>
	/// <param name="playerCount">The number of players to include in the group. Must be a non-negative integer.</param>
	/// <returns>A <see cref="Group"/> instance containing the specified number of players.</returns>
	private static Group CreateGroupWithPlayers(int playerCount)
	{
		List<Player> players = [..
			Enumerable.Range(0, playerCount)
			.Select(i => Player.Create($"Player {i}", i * 2))];

		return Group.Create("Test Group", players);
	}

	[Fact]
	public void Group_Matches_And_Result()
	{
		int noOfPlayers = 4;

		Group group = CreateGroupWithPlayers(4);
		group.Players.Count.ShouldBe(noOfPlayers);

		group.Matches.Count.ShouldBe(6);
		
		group.Matches[0] = group.Matches[0].SetResult((21, 12), (21, 14));
		group.Matches[1] = group.Matches[1].SetResult((21, 12), (14, 21), (21, 19));
		group.Matches[2] = group.Matches[2].SetResult((21, 12), (14, 21), (21, 19));
		group.Matches[3] = group.Matches[3].SetResult((28, 30), (14, 21));
		group.Matches[4] = group.Matches[4].SetResult((21, 12), (14, 21), (21, 19));
		group.Matches[5] = group.Matches[5].SetResult((21, 12), (14, 21));


		// Check if all matches have results
		foreach (Match match in group.Matches) {
			_ = match.Result.ShouldNotBeNull();

			match.Result.PlayerASets.ShouldBeGreaterThanOrEqualTo(0);
			match.Result.PlayerASets.ShouldBeLessThanOrEqualTo(30);
			match.Result.PlayerBSets.ShouldBeGreaterThanOrEqualTo(0);
			match.Result.PlayerBSets.ShouldBeLessThanOrEqualTo(30);
		}

		group.Matches[0].IsCompleted.ShouldBeTrue();
		group.Matches[1].IsCompleted.ShouldBeTrue();
		group.Matches[2].IsCompleted.ShouldBeTrue();
		group.Matches[3].IsCompleted.ShouldBeTrue();
		group.Matches[4].IsCompleted.ShouldBeTrue();
		group.Matches[5].IsCompleted.ShouldBeFalse();

		group.Matches[5] = group.Matches[5].SetResult((21, 12), (14, 21), (19, 21));
		group.Matches[5].IsCompleted.ShouldBeTrue();

		testOutputHelper.WriteLine(group.AsString());

		group.Matches[0].IsPlayerAWin.ShouldBeTrue();
		group.Matches[1].IsPlayerAWin.ShouldBeTrue();
		group.Matches[2].IsPlayerAWin.ShouldBeTrue();
		group.Matches[3].IsPlayerBWin.ShouldBeTrue();
		group.Matches[4].IsPlayerAWin.ShouldBeTrue();
		group.Matches[5].IsPlayerBWin.ShouldBeTrue();

		group.GroupPositions.Count.ShouldBe(noOfPlayers);
		group.GroupPositions[0].Player.Name.ShouldBe(group.Players[0].Name);
		group.GroupPositions[1].Player.Name.ShouldBe(group.Players[1].Name);
		group.GroupPositions[2].Player.Name.ShouldBe(group.Players[3].Name);
		group.GroupPositions[3].Player.Name.ShouldBe(group.Players[2].Name);

	}
}