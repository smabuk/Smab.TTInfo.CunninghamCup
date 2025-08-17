using Xunit.Abstractions;

namespace Smab.TTInfo.CunninghamCup.Tests;

public class GroupTests(ITestOutputHelper testOutputHelper)
{
	private Group CreateGroupWithPlayers(int playerCount)
	{
		List<Player> players = [];
		for (int i = 0; i < playerCount; i++) {
			Player player = Player.Create(($"Player {i + 1}"), i);
			players.Add(player);
		}

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
		group.Matches[3] = group.Matches[3].SetResult((21, 12), (14, 21), (21, 19));
		group.Matches[4] = group.Matches[4].SetResult((21, 12), (14, 21), (21, 19));
		group.Matches[5] = group.Matches[5].SetResult((21, 12), (14, 21));

		testOutputHelper.WriteLine(group.AsString());

		// Check if all matches have results
		foreach (Match match in group.Matches) {
			_ = match.Result.ShouldNotBeNull();

			match.Result.PlayerAScore.ShouldBeGreaterThanOrEqualTo(0);
			match.Result.PlayerAScore.ShouldBeLessThanOrEqualTo(30);
			match.Result.PlayerBScore.ShouldBeGreaterThanOrEqualTo(0);
			match.Result.PlayerBScore.ShouldBeLessThanOrEqualTo(30);
		}

		group.Matches[0].IsCompleted.ShouldBeTrue();
		group.Matches[1].IsCompleted.ShouldBeTrue();
		group.Matches[2].IsCompleted.ShouldBeTrue();
		group.Matches[3].IsCompleted.ShouldBeTrue();
		group.Matches[4].IsCompleted.ShouldBeTrue();
		group.Matches[5].IsCompleted.ShouldBeFalse();

		group.Matches[5] = group.Matches[5].SetResult((21, 12), (14, 21), (19, 21));
		group.Matches[5].IsCompleted.ShouldBeTrue();


		group.Matches[0].IsPlayerAWin.ShouldBeTrue();
		group.Matches[1].IsPlayerAWin.ShouldBeTrue();
		group.Matches[2].IsPlayerAWin.ShouldBeTrue();
		group.Matches[3].IsPlayerAWin.ShouldBeTrue();
		group.Matches[4].IsPlayerAWin.ShouldBeTrue();
		group.Matches[5].IsPlayerBWin.ShouldBeTrue();



	}
}