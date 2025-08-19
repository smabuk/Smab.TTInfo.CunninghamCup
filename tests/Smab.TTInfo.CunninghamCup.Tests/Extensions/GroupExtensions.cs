using Xunit.Sdk;

namespace Smab.TTInfo.CunninghamCup.Tests.Extensions;
public static class GroupExtensions
{
	extension(Group group)
	{
		public Group CompleteWithRandomResults()
		{
			List<Match> matches = [.. group.Matches];
			for (int i = 0; i < matches.Count; i++) {
				if (!matches[i].IsCompleted) {
					matches[i] = matches[i].SetRandomResult();
				}
			}

			return group with { Matches = [.. matches] };
		}
	}
}
