using Model.Entities;

namespace Model.Repositories
{
    public interface IStandingRepository
    {
        Task<List<Standing>> GetByTournamentAsync(int tournamentId);
        Task<Standing> GetAsync(int id, int clubId);
        Task<int> InsertAsync(Standing item);
        Task<int> InsertAsync(ICollection<Standing> list);
        Task<int> UpdateAsync(Standing item);
    }
}
