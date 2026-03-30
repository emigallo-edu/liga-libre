using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class UpdateClub
    {
        private readonly IClubRepository _clubRepository;

        public UpdateClub(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task ExecuteAsync(Club club)
        {
            await _clubRepository.UpdateAsync(club);
        }
    }
}
