using System.Text;

namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;

public static class KnockoutRoundExtensions
{
	extension(KnockoutRound)
	{
	}

	extension(KnockoutRound knockoutRound)
	{
		public string AsString()
		{
			StringBuilder sb = new();
			_ = sb.AppendLine($"Round: {knockoutRound.Type.ToFriendlyString()}");

			foreach (Match match in knockoutRound.Matches) {
				string playerA = $"{match.PlayerA.DisplayName,-18}";
				string playerB = $"{match.PlayerB.DisplayName,-18}";
				if (match.PlayerA.IsPlayer && match.PlayerB.IsPlayer) {
					playerA = $$"""{{playerA.Trim(),-12}} {{$"({match.PlayerAStart,3})"}}""";
					playerB = $$"""{{playerB.Trim(),-12}} {{$"({match.PlayerBStart,3})"}}""";
				}

				_ = sb.Append($"- {playerA} vs {playerB}");
				if (match.IsCompleted) {
					_ = sb.Append($"   Winner {match.Winner?.DisplayName,-12} {match.Result?.AsString()}");
				}

				_ = sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}