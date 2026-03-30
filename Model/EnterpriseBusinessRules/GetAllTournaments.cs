using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetAllTournaments
    {
        private readonly ITournamentRepository _tournamentRepository;

        public GetAllTournaments(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<List<Tournament>> ExecuteAsync()
        {
            return await _tournamentRepository.GetAllAsync();
        }
    }
}
