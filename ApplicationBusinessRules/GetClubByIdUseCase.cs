using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetClubByIdUseCase
    {
        private readonly GetClubById _getClubById;

        public GetClubByIdUseCase(GetClubById getClubById)
        {
            _getClubById = getClubById;
        }

        public async Task<Club> ExecuteAsync(int id)
        {
            return await _getClubById.ExecuteAsync(id);
        }
    }
}
