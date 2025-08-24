namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static partial class TournamentExtensions
{
	extension(Tournament tournament)
	{
		public Tournament? Load(string filePath, string cacheFolder)
		{
			string? json = CacheHelper.LoadFileFromCache($"{filePath}", cacheFolder);
			return JsonSerializer.Deserialize<Tournament>(json ?? "");
		}
	}

	extension(Tournament tournament)
	{
		public bool Save(string filePath, string cacheFolder)
		{
			string contents = JsonSerializer.Serialize(tournament);

			return CacheHelper.SaveFileToCache(contents, $"{filePath}", cacheFolder);
		}
	}
}