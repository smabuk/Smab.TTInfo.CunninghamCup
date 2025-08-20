namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;
public static class PlayerIdExtensions
{
	extension(PlayerId playerId)
	{
		public string DisplayName => playerId.IsBye || playerId.IsPlaceHolder
			? playerId.StringId[1..]
			: playerId.StringId;
	}
}
