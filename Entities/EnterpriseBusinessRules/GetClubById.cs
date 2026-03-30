using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetClubById
    {
        private readonly IClubRepository _clubRepository;

        public GetClubById(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<Club> ExecuteAsync(int id)
        {
            return await _clubRepository.GetByIdAsync(id);
        }
    }
}
