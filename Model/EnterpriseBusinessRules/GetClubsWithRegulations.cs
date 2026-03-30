using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetClubsWithRegulations
    {
        private readonly IClubRepository _clubRepository;

        public GetClubsWithRegulations(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<List<Club>> ExecuteAsync()
        {
            return await _clubRepository.GetClubsWithRegulations();
        }
    }
}
