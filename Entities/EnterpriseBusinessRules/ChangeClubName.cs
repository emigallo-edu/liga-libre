using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class ChangeClubName
    {
        private readonly IClubRepository _clubRepository;

        public ChangeClubName(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task ExecuteAsync(int clubId, string newName)
        {
            await _clubRepository.ChangeName(clubId, newName);
        }
    }
}
