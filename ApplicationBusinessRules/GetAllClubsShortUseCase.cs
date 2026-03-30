using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetAllClubsShortUseCase
    {
        private readonly GetAllClubsShort _getAllClubsShort;

        public GetAllClubsShortUseCase(GetAllClubsShort getAllClubsShort)
        {
            _getAllClubsShort = getAllClubsShort;
        }

        public async Task<List<ShortClub>> ExecuteAsync()
        {
            return await _getAllClubsShort.ExecuteAsync();
        }
    }
}
