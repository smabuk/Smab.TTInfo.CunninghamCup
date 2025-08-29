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
		public Tournament DrawMainKnockoutStage(bool redraw = false)
		{
			Tournament tournament = tournamentService.GetTournament();
			// TODO: FIX: currently hardcoded to 1st and 2nd place in groups
			tournament = tournament  with {
				KnockoutStage = tournament.DrawKnockoutStage("Main Knockout", [0, 1], redraw)
			};
			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}

		public Tournament DrawConsolationStage(bool redraw = false)
		{
			Tournament tournament = tournamentService.GetTournament();
			// TODO: FIX: currently hardcoded to 3rd and 4th place in groups
			tournament = tournament with {
				ConsolationStage = tournament.DrawKnockoutStage("Consolation", [2, 3], redraw)
			};
			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}

		public Tournament UpdateMainKnockoutRounds()
		{
			Tournament tournament = tournamentService.GetTournament();
			if (tournament.KnockoutStage is null) {
				return tournament;
			}

			bool success = tournament.TryDrawKnockoutStage(tournament.KnockoutStage, out tournament, out string? _);
			if (!success) {
				throw new InvalidOperationException("Failed to update knockout phases.");
			}

			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}

		public Tournament UpdateConsolationRounds()
		{
			Tournament tournament = tournamentService.GetTournament();
			if (tournament.ConsolationStage is null) {
				return tournament;
			}

			bool success = tournament.TryDrawKnockoutStage(tournament.ConsolationStage, out tournament, out string? _);
			if (!success) {
				throw new InvalidOperationException("Failed to update knockout phases.");
			}

			tournamentService.AddOrUpdateTournament(tournament);
			return tournament;
		}
	}
}
