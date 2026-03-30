using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class UpdateClubWithStadium
    {
        private readonly IClubRepository _clubRepository;

        public UpdateClubWithStadium(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task ExecuteAsync(List<Club> clubs)
        {
            await _clubRepository.UpdateWithStadiumAsync(clubs);
        }
    }
}
