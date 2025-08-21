namespace Smab.TTInfo.CunninghamCup.Shared.TTLeagues;

public interface ITournamentService
{
	Tournament? GetTournament(string name);
	Tournament? GetTournament(Guid id);
	IEnumerable<Tournament> GetAllTournaments();
	void AddOrUpdateTournament(Tournament tournament);
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
}
