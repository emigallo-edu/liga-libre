using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetStanding
    {
        private readonly IStandingRepository _standingRepository;

        public GetStanding(IStandingRepository standingRepository)
        {
            _standingRepository = standingRepository;
        }

        public async Task<Standing> ExecuteAsync(int id, int clubId)
        {
            return await _standingRepository.GetAsync(id, clubId);
        }
    }
}
