using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetTournamentByIdUseCase
    {
        private readonly GetTournamentById _getTournamentById;

        public GetTournamentByIdUseCase(GetTournamentById getTournamentById)
        {
            _getTournamentById = getTournamentById;
        }

        public async Task<Tournament> ExecuteAsync(int id)
        {
            return await _getTournamentById.ExecuteAsync(id);
        }
    }
}
