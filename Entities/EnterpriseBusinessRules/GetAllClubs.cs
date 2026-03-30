using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetAllClubs
    {
        private readonly IClubRepository _clubRepository;

        public GetAllClubs(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<List<Club>> ExecuteAsync()
        {
            return await _clubRepository.GetAllAsync();
        }
    }
}