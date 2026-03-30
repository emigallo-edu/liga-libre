using Model.Entities;

namespace Model.Repositories
{
    public interface ITournamentRepository
    {
        Task<int> InsertAsync(Tournament item);
        Task<Tournament> GetByIdAsync(int id);
        Task<List<Tournament>> GetAllAsync();
    }
}