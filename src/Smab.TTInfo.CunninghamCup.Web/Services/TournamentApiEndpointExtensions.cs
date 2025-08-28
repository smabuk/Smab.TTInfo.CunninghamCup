namespace Smab.TTInfo.CunninghamCup.Web.Services;

public static class TournamentApiEndpointExtensions
{
	public static IEndpointRouteBuilder MapTournamentApiEndpoints(this IEndpointRouteBuilder app)
	{
		TournamentApiEndpoints.MapTournamentApiEndpoints(app);
		return app;
	}
}
