using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetMatchesByTournament
    {
        private readonly IMatchRepository _matchRepository;

        public GetMatchesByTournament(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<List<Match>> ExecuteAsyncV1(int tournamentId)
        {
            return await _matchRepository.GetByTournamentAsyncV1(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV2(int tournamentId)
        {
            return await _matchRepository.GetByTournamentAsyncV2(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV3(int tournamentId)
        {
            return await _matchRepository.GetByTournamentAsyncV3(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV4(int tournamentId)
        {
            return await _matchRepository.GetByTournamentAsyncV4(tournamentId);
        }
    }
}
