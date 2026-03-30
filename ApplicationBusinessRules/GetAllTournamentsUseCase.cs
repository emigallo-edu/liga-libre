using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetAllTournamentsUseCase
    {
        private readonly GetAllTournaments _getAllTournaments;

        public GetAllTournamentsUseCase(GetAllTournaments getAllTournaments)
        {
            _getAllTournaments = getAllTournaments;
        }

        public async Task<List<Tournament>> ExecuteAsync()
        {
            return await _getAllTournaments.ExecuteAsync();
        }
    }
}
