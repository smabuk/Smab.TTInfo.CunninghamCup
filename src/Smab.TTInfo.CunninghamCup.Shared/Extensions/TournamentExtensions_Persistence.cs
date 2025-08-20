using System.Text.Json;

namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static partial class TournamentExtensions
{
	extension(Tournament tournament)
	{
		public Tournament? Load(string filePath)
		{
			string? json = File.Exists(filePath) ? File.ReadAllText(filePath) : null;
			return JsonSerializer.Deserialize<Tournament>(json ?? "");
		}
	}

	extension(Tournament tournament)
	{
		public bool Save(string filePath)
		{
			string json = JsonSerializer.Serialize(tournament);

			try {
				File.WriteAllText(filePath, json);
			}
			catch (Exception) {
				//return false;
				throw;
			}

			return true;
		}
	}
}