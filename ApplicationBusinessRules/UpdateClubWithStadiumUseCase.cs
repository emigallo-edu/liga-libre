using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class UpdateClubWithStadiumUseCase
    {
        private readonly UpdateClubWithStadium _updateClubWithStadium;

        public UpdateClubWithStadiumUseCase(UpdateClubWithStadium updateClubWithStadium)
        {
            _updateClubWithStadium = updateClubWithStadium;
        }

        public async Task ExecuteAsync(List<Club> clubs)
        {
            await _updateClubWithStadium.ExecuteAsync(clubs);
        }
    }
}
