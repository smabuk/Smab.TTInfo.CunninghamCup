using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.CunninghamCup.Shared.Services;

public interface ITournamentService
{
	Tournament GetTournament();
	void AddOrUpdateTournament(Tournament tournament);

	Task<Tournament> LoadTournamentFromJsonAsync(string filePath);
	public Task<bool> SaveTournamentToJsonAsync(string filePath);
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
