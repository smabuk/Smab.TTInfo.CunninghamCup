﻿namespace Smab.TTInfo.CunninghamCup.Shared.TTLeagues.Services;

/// <summary>
/// Provides functionality to read files from the TTLeagues cache.
/// </summary>
public sealed partial class TTLeaguesReader
{
	/// <summary>
	/// Loads a file from the cache for a given file name.
	/// </summary>
	/// <param name="fileName">The name of the file to load from the cache.</param>
	/// <param name="cacheFolder">The folder where the cache files are stored. If <c>null</c>, the default cache folder is used.</param>
	/// <returns>The cached file content as a string, or <c>null</c> if not found.</returns>
	public string? LoadFileFromCache(string fileName, string? cacheFolder = null)
		=> CacheHelper.LoadFileFromCache($"{CACHEFILE_PREFIX}{fileName}", cacheFolder ?? CacheFolder);
}
