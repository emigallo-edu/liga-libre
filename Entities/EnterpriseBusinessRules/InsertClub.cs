using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class InsertClub
    {
        private readonly IClubRepository _clubRepository;

        public InsertClub(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<int> ExecuteAsync(Club club)
        {
            return await _clubRepository.InsertAsync(club);
        }
    }
}