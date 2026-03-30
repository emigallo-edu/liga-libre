using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetMatch
    {
        private readonly IMatchRepository _matchRepository;

        public GetMatch(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<Match> ExecuteAsync(int id)
        {
            return await _matchRepository.GetAsync(id);
        }
    }
}
