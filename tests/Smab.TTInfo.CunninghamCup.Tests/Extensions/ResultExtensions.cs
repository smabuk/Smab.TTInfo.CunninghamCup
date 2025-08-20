namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;
public static class ResultExtensions
{
	extension(Result? result)
	{
		public string AsString()
		{
			string resultString = "";
			if (result is null) {
				return "";
			}

			if (result.Sets.Count > 0) {
				resultString = string.Join(" ", result.Sets.Select(s => $"{s.PlayerAScore}-{s.PlayerBScore}"));
			}

			return resultString;
		}
	}
}
