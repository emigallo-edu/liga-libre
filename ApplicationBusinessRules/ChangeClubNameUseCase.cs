using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class ChangeClubNameUseCase
    {
        private readonly Model.EnterpriseBusinessRules.ChangeClubName _changeClubName;

        public ChangeClubNameUseCase(Model.EnterpriseBusinessRules.ChangeClubName changeClubName)
        {
            _changeClubName = changeClubName;
        }

        public async Task ExecuteAsync(int clubId, string newName)
        {
            await _changeClubName.ExecuteAsync(clubId, newName);
        }
    }
}
