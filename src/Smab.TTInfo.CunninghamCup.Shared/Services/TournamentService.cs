namespace Smab.TTInfo.CunninghamCup.Shared.Services;

public interface ITournamentService
{
	Tournament? GetTournament(string name);
	Tournament? GetTournament(Guid id);
	IEnumerable<Tournament> GetAllTournaments();
	void AddOrUpdateTournament(Tournament tournament);

	Task<IEnumerable<Tournament>> LoadTournamentsFromJsonAsync(string filePath);
	Tournament SeedRandomTournament();
}

public class TournamentService : ITournamentService
{
	private readonly Dictionary<Guid, Tournament> _tournaments = [];

	public Tournament? GetTournament(Guid id)
		=> _tournaments.TryGetValue(id, out Tournament? tournament) ? tournament : null;

	public Tournament? GetTournament(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) {
			return null;
		}

		// Assuming _tournaments is a Dictionary<???, Tournament>
		return _tournaments.Values.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	public IEnumerable<Tournament> GetAllTournaments() => _tournaments.Values;

	public void AddOrUpdateTournament(Tournament tournament) => _tournaments[tournament.Id] = tournament;

	public Task<IEnumerable<Tournament>> LoadTournamentsFromJsonAsync(string filePath)
	{
		// Implementation for loading tournaments from a JSON file
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
