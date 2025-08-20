namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;
public static class PlayerIdExtensions
{
	extension(PlayerId)
	{
		public static PlayerId Empty => new("");
		public static PlayerId Bye   => new("*BYE");
		public static string PlaceHolderSymbol => "|";

	}
	extension(PlayerId playerId)
	{
		public bool IsBye => playerId == PlayerId.Bye;
		public bool IsPlaceHolder => playerId.StringId.StartsWith(PlayerId.PlaceHolderSymbol);
		public bool IsPlayer => !(playerId.IsBye || playerId.IsPlaceHolder);
		public bool IsWithdrawn => playerId.StringId.StartsWith("-|");
	}
}
