using System.Text;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static class KnockoutRoundExtensions
{
	extension(KnockoutRound)
	{
	}

	extension(KnockoutRound knockoutRound)
	{
		public bool IsCompleted => knockoutRound.Matches.All(match => match.IsCompleted);
		public bool IsNotPopulated => !knockoutRound.IsPopulated;
		public bool IsPopulated => knockoutRound.Matches is not []
		&& knockoutRound.Matches
			.All(match =>	(match.PlayerA.IsPlayer || match.PlayerA.IsBye)
						&&	(match.PlayerB.IsPlayer || match.PlayerB.IsBye));

		public string AsString()
		{
			StringBuilder sb = new();
			_ = sb.AppendLine($"Round: {knockoutRound.Name}");

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