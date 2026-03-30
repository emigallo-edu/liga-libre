using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class UpdateStanding
    {
        private readonly IStandingRepository _standingRepository;

        public UpdateStanding(IStandingRepository standingRepository)
        {
            _standingRepository = standingRepository;
        }

        public async Task<int> ExecuteAsync(Standing item)
        {
            return await _standingRepository.UpdateAsync(item);
        }
    }
}
