using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetClubsWithRegulationsUseCase
    {
        private readonly GetClubsWithRegulations _getClubsWithRegulations;

        public GetClubsWithRegulationsUseCase(GetClubsWithRegulations getClubsWithRegulations)
        {
            _getClubsWithRegulations = getClubsWithRegulations;
        }

        public async Task<List<Club>> ExecuteAsync()
        {
            return await _getClubsWithRegulations.ExecuteAsync();
        }
    }
}
