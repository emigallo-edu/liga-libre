using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Repositories;

namespace Repository
{
    public class StadiumRepository : IStadiumRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public StadiumRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }

        public async Task<Stadium> GetByIdAsync(string name)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                return await context.Stadiums
                    .FirstAsync(x => x.Name == name);
            }
        }
    }
}