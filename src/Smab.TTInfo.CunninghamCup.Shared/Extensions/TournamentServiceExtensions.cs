namespace Smab.TTInfo.CunninghamCup.Shared.Extensions;

public static partial class TournamentServiceExtensions
{
	extension(ITournamentService tournamentService)
	{
		public ITournamentService DrawGroups(int groupSize)
		{
			Tournament tournament = tournamentService.GetTournament();
			tournament = tournament.DrawGroups(groupSize);
			tournamentService.AddOrUpdateTournament(tournament);
			return tournamentService;
		}
	}

	extension(ITournamentService tournamentService)
	{
		public Tournament DrawKnockoutStage(bool redraw = false)
		{
			Tournament tournament = tournamentService.GetTournament();
			tournament = tournament 
				with { KnockoutStage = tournament.DrawKnockoutStage("Main Knockout", redraw) };
			tournament = tournament 
				with { ConsolationStage = tournament.DrawKnockoutStage("Consolation", redraw) };
			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}
	}

	extension(ITournamentService tournamentService)
	{
		public Tournament UpdateKnockoutPhases()
		{
			Tournament tournament = tournamentService.GetTournament();
			bool success = tournament.TryDrawKnockoutStage(tournament.KnockoutStage!, out tournament, out string? _);
			if (!success) {
				throw new InvalidOperationException("Failed to update knockout phases.");
			}

			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}
	}
}
