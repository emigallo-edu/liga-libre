using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetStandingsByTournamentUseCase
    {
        private readonly GetStandingsByTournament _getStandingsByTournament;

        public GetStandingsByTournamentUseCase(GetStandingsByTournament getStandingsByTournament)
        {
            _getStandingsByTournament = getStandingsByTournament;
        }

        public async Task<List<Standing>> ExecuteAsync(int tournamentId)
        {
            return await _getStandingsByTournament.ExecuteAsync(tournamentId);
        }
    }
}
