using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetMatchesByTournamentUseCase
    {
        private readonly GetMatchesByTournament _getMatchesByTournament;

        public GetMatchesByTournamentUseCase(GetMatchesByTournament getMatchesByTournament)
        {
            _getMatchesByTournament = getMatchesByTournament;
        }

        public async Task<List<Match>> ExecuteAsyncV1(int tournamentId)
        {
            return await _getMatchesByTournament.ExecuteAsyncV1(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV2(int tournamentId)
        {
            return await _getMatchesByTournament.ExecuteAsyncV2(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV3(int tournamentId)
        {
            return await _getMatchesByTournament.ExecuteAsyncV3(tournamentId);
        }

        public async Task<List<Match>> ExecuteAsyncV4(int tournamentId)
        {
            return await _getMatchesByTournament.ExecuteAsyncV4(tournamentId);
        }
    }
}
