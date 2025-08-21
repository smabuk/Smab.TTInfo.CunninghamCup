namespace Smab.TTInfo.CunninghamCup.Shared.Services;

public interface ITournamentService
{
	Tournament GetTournament();
	void AddOrUpdateTournament(Tournament tournament);

	Task<Tournament> LoadTournamentFromJsonAsync(string filePath);
	public Task<bool> SaveTournamentToJsonAsync(string filePath);

	Tournament SeedRandomTournament();
}

public class TournamentService : ITournamentService
{
	private Tournament? _tournament;

	public Tournament GetTournament()
		=> _tournament ?? throw new InvalidOperationException("Tournament not initialised.");

	public void AddOrUpdateTournament(Tournament tournament) => _tournament = tournament;

	public Task<Tournament> LoadTournamentFromJsonAsync(string filePath)
	{
		// Implementation for loading a tournament from a JSON file
		throw new NotImplementedException();
	}

	public Task<bool> SaveTournamentToJsonAsync(string filePath)
	{
		// Implementation for saving a tournament to a JSON file
		throw new NotImplementedException();
	}

	public Tournament SeedRandomTournament()
	{
		Tournament tournament = Tournament.Create(
			name: "Test Cunningham Cup",
			date: DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
			players: [.. Enumerable.Range(1, 15)
				.Select(i => Player.Create($"Player {i}", Random.Shared.Next(-10, 10)))]
		);

		AddOrUpdateTournament(tournament);

		return tournament;
	}
}
