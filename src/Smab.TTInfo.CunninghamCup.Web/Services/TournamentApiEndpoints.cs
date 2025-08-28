using Smab.TTInfo.CunninghamCup.Shared.Services;

using System.Text.Json;

namespace Smab.TTInfo.CunninghamCup.Web.Services;

public static class TournamentApiEndpoints
{
	public static void MapTournamentApiEndpoints(IEndpointRouteBuilder endpoints)
	{
		_ = endpoints
			.MapGet("/api/tournament/download", DownloadTournamentAsync)
			.WithName("DownloadTournament");

		_ = endpoints
			.MapPost("/api/tournament/upload", UploadTournamentAsync)
			.WithName("UploadTournament");
	}

	private static Task<IResult> DownloadTournamentAsync(ITournamentService tournamentService)
	{
		Tournament tournament;
		try {
			tournament = tournamentService.GetTournament();
		}
		catch (InvalidOperationException) {
			return Task.FromResult<IResult>(Results.NotFound());
		}

		return Task.FromResult<IResult>(
			TypedResults.File(
				System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(tournament)),
			"text/json",
			$"tournament_2025_{DateTime.Now:yyyyMMdd-HHmmss}.json"));
		//return Task.FromResult<IResult>(Results.Json(tournament));
	}

	private static async Task<IResult> UploadTournamentAsync(HttpRequest request, ITournamentService tournamentService)
	{
		if (!request.HasFormContentType) {
			return Results.BadRequest("Form content type required.");
		}

		IFormCollection form = await request.ReadFormAsync();
		IFormFile? file = form.Files["file"];
		if (file is null || file.Length == 0) {
			return Results.BadRequest("No file uploaded.");
		}

		using Stream stream = file.OpenReadStream();
		try {
			Tournament? tournament = await JsonSerializer.DeserializeAsync<Tournament>(stream);
			if (tournament is null) {
				return Results.BadRequest("Invalid tournament data.");
			}

			tournamentService.AddOrUpdateTournament(tournament);
			bool saved = await tournamentService.SaveTournamentToJsonAsync();
			return saved ? Results.Ok() : Results.StatusCode(500);
		}
		catch (JsonException ex) {
			return Results.BadRequest($"Invalid JSON: {ex.Message}");
		}
	}
}
