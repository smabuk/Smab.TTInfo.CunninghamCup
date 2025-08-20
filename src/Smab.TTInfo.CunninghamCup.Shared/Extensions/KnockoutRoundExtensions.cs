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
	}
}