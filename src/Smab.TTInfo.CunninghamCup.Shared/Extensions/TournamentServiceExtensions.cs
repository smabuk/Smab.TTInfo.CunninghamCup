using Smab.TTInfo.CunninghamCup.Shared.Services;

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
}
