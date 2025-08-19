namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class PlayerIdExtensions
{
	extension(PlayerId playerId)
	{
		public bool IsBye => playerId.StringId.StartsWith("BYE");
		public bool IsPlaceHolder => playerId.StringId.StartsWith("|");
		public bool IsPlayer => !(playerId.IsBye || playerId.IsPlaceHolder);
		public bool IsWithdrawn => playerId.StringId.StartsWith("-|");
	}
}
