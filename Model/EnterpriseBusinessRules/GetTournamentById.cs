using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetTournamentById
    {
        private readonly ITournamentRepository _tournamentRepository;

        public GetTournamentById(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<Tournament> ExecuteAsync(int id)
        {
            return await _tournamentRepository.GetByIdAsync(id);
        }
    }
}
