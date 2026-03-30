using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetStandingsByTournament
    {
        private readonly IStandingRepository _standingRepository;

        public GetStandingsByTournament(IStandingRepository standingRepository)
        {
            _standingRepository = standingRepository;
        }

        public async Task<List<Standing>> ExecuteAsync(int tournamentId)
        {
            return await _standingRepository.GetByTournamentAsync(tournamentId);
        }
    }
}
