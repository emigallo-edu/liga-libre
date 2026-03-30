using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class InsertMatchResult
    {
        private readonly IMatchRepository _matchRepository;

        public InsertMatchResult(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<int> ExecuteAsync(MatchResult matchResult)
        {
            return await _matchRepository.InsertMatchResult(matchResult);
        }
    }
}
