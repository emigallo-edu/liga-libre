using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetAllClubsUseCase
    {
        private readonly GetAllClubs _getAllClubs;

        public GetAllClubsUseCase(GetAllClubs getAllClubs)
        {
            _getAllClubs = getAllClubs;
        }

        public async Task<List<Club>> ExecuteAsync()
        {
            var result = await _getAllClubs.ExecuteAsync();

            foreach (var item in result.Where(x => x.Stadium != null))
            {
                item.Stadium.Club = null;
            }

            return result;
        }
    }
}
