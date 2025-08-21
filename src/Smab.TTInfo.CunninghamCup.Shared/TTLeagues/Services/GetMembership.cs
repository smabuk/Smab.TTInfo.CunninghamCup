namespace Smab.TTInfo.CunninghamCup.Shared.TTLeagues.Services;

/// <summary>
/// Provides functionality to read match data from TTLeagues.
/// </summary>
public sealed partial class TTLeaguesReader
{
	public async Task<Membership?> GetMembership(string ttinfoId, int id)
	{
		string fileName = $"{ttinfoId}_membership_{id}.json";

		return await LoadJsonAsync<Membership>(
			ttinfoId,
			$"memberships/members?count&membershipId={id}",
			fileName,
			cacheHours: 0
			);
	}
}