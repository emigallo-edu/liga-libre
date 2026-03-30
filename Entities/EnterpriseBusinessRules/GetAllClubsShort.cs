using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetAllClubsShort
    {
        private readonly IClubRepository _clubRepository;

        public GetAllClubsShort(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<List<ShortClub>> ExecuteAsync()
        {
            return await _clubRepository.GetAllShortAsync();
        }
    }
}
