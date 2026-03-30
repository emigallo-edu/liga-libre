using Model.Entities;

namespace Model.Repositories
{
    public interface IMatchRepository
    {
        Task<Match> GetAsync(int id);
        Task<List<Match>> GetByTournamentAsyncV1(int? tournamentId, bool sortAscending = true);
        Task<List<Match>> GetByTournamentAsyncV2(int tournamentId);
        Task<List<Match>> GetByTournamentAsyncV3(int tournamentId);
        Task<List<Match>> GetByTournamentAsyncV4(int tournamentId);
        Task<int> InsertMatchResult(MatchResult item);
    }
}
