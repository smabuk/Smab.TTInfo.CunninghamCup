namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class PlayerIdExtensions
{
	extension(PlayerId playerId)
	{
		public bool IsBye => playerId.stringId.StartsWith("BYE");
		public bool IsPlaceHolder => playerId.stringId.StartsWith("|");
		public bool IsPlayer => !(playerId.IsBye || playerId.IsPlaceHolder);
		public bool IsWithdrawn => playerId.stringId.StartsWith("-|");
	}
}
