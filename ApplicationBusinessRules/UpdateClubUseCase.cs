using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class UpdateClubUseCase
    {
        private readonly UpdateClub _updateClub;

        public UpdateClubUseCase(UpdateClub updateClub)
        {
            _updateClub = updateClub;
        }

        public async Task ExecuteAsync(Club club)
        {
            await _updateClub.ExecuteAsync(club);
        }
    }
}
