using Model.Entities;

namespace Model.Repositories
{
    public interface IStadiumRepository
    {
        Task<Stadium> GetByIdAsync(string name);
    }
}