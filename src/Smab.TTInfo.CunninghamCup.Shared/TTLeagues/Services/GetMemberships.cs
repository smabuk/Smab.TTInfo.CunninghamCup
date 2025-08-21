using Smab.TTInfo.CunninghamCup.Shared.TTLeagues.Models;

namespace Smab.TTInfo.CunninghamCup.Shared.TTLeagues.Services;

/// <summary>
/// Provides functionality to read match data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	public async Task<Memberships?> GetMemberships(string ttinfoId)
	{
		string fileName = $"{ttinfoId}_memberships.json";

		return await LoadJsonAsync<Memberships>(
			ttinfoId,
			$"memberships?public=true",
			fileName);
	}
}