using Microsoft.Extensions.DependencyInjection;

namespace Smab.TTInfo.CunninghamCup.Shared.Services;

public interface ITournamentService
{
	Tournament GetTournament();
	void AddOrUpdateTournament(Tournament tournament);

	Task<Tournament> LoadTournamentFromJsonAsync();
	public Task<bool> SaveTournamentToJsonAsync(string? stage = null);
	public Task<bool> LogToAuditFile(string message);
}

public class TournamentService(TTInfoOptions ttinfoOptions) : ITournamentService
{
	private Tournament? _tournament;
	private string CacheFolder { get; set; } = ttinfoOptions.CacheFolder;

	public Tournament GetTournament()
		=> _tournament ?? throw new InvalidOperationException("Tournament not initialised.");

	public void AddOrUpdateTournament(Tournament tournament) => _tournament = tournament;

	public async Task<Tournament> LoadTournamentFromJsonAsync()
	{
		_tournament ??= Tournament.Create(
				name: "Placeholder",
				date: DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
				players: []
			);

		_tournament = await Task.Run(() => _tournament.Load($"tournament_{DateTime.Now.Year}.json", CacheFolder) ?? _tournament);
		return _tournament ?? throw new InvalidOperationException("Tournament not initialised.");
	}

	public async Task<bool> SaveTournamentToJsonAsync(string? stage = null)
	{
		if (_tournament is null) {
			return false;
		}

		// if stage is provided, include it in the filename
		if (!string.IsNullOrWhiteSpace(stage)) {
			_ = await Task.Run(() => _tournament.Save($"tournament_{_tournament.Name}_{DateTime.Now:yyyyMMdd-HHmmss}_{stage.Replace(' ', '_')}.json", CacheFolder));
			_ = LogToAuditFile($"Tournament saved: {_tournament.Name} at stage {stage}");
		} else {
			_ = await Task.Run(() => _tournament.Save($"tournament_{_tournament.Name}_{DateTime.Now:yyyyMMdd-HHmmss}.json", CacheFolder));
			_ = LogToAuditFile($"Tournament saved: {_tournament.Name}");
		}

		return await Task.Run(() => _tournament.Save($"tournament_{DateTime.Now.Year}.json", CacheFolder));
	}


	public async Task<bool> LogToAuditFile(string messsage)
	{
		string logMessage = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ssZ} - {messsage}{Environment.NewLine}";
		return await Task.Run(() => CacheHelper.AppendTextToFileInCache(logMessage, $"tournament_{DateTime.Now.Year}.log", CacheFolder));
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
