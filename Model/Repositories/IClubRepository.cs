using Model.Entities;

namespace Model.Repositories
{
    public interface IClubRepository
    {
        Task<Club> GetByIdAsync(int id);
        Task<List<Club>> GetAllAsync();
        Task<List<ShortClub>> GetAllShortAsync();
        Task<int> InsertAsync(Club club);
        Task ChangeName(int cludId, string newName);
        Task UpdateAsync(Club club);
        Task UpdateWithStadiumAsync(List<Club> clubs);
        Task<List<Club>> GetClubsWithRegulations();
    }
}