using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class MigrationRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public MigrationRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }

        public async Task<IEnumerable<string>> MigrateAsync()
        {
            using (var db = new ApplicationDbContext(this._options))
            {
                var before = await db.Database.GetAppliedMigrationsAsync();
                var pendings = await db.Database.GetPendingMigrationsAsync();
                if (pendings.Any())
                {
                    await db.Database.MigrateAsync();
                }
                var after = await db.Database.GetAppliedMigrationsAsync();
                return pendings.Intersect(after);
            }
        }

        public async Task<IEnumerable<string>> GetPendingMigrations()
        {
            using (var db = new ApplicationDbContext(this._options))
            {
                return await db.Database.GetPendingMigrationsAsync();
            }
        }
    }
}