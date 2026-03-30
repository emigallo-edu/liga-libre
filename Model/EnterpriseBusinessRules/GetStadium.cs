using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetStadium
    {
        private readonly IStadiumRepository _stadiumRepository;

        public GetStadium(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        public async Task<Stadium> ExecuteAsync(string name)
        {
            return await _stadiumRepository.GetByIdAsync(name);
        }
    }
}
