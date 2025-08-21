using Microsoft.Extensions.DependencyInjection;

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

		tournament = tournament.DrawGroups(4);

		AddOrUpdateTournament(tournament);

		return tournament;
	}
}


public static class TournamentServiceExtensions
{
	public static IServiceCollection? AddTournamentService(this IServiceCollection? services, string configSectionName = "TTInfo")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		if (string.IsNullOrWhiteSpace(configSectionName)) {
			throw new ArgumentException($"'{nameof(configSectionName)}' cannot be null or whitespace.", nameof(configSectionName));
		}

		_ = services.AddOptions<TTInfoOptions>()
			.BindConfiguration(configSectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		return services.AddSingleton<ITournamentService, TournamentService>();
	}

	public static IServiceCollection? AddTournamentService(this IServiceCollection? services, Action<TTInfoOptions> options, string configSectionName = "TTInfo")
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		_ = services.AddTournamentService(configSectionName);
		_ = services.PostConfigure(options);

		return services;
	}
}
