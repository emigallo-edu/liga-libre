using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class InsertTournament
    {
        private readonly ITournamentRepository _tournamentRepository;

        public InsertTournament(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<int> ExecuteAsync(Tournament tournament)
        {
            return await _tournamentRepository.InsertAsync(tournament);
        }
    }
}
