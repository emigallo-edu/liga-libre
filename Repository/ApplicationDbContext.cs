using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchResult> MatchsResults { get; set; }
        public DbSet<Stadium> Stadiums { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Standing> Standings { get; set; }
        public DbSet<ResponseAudit> ResponseAudits { get; set; }
        public DbSet<Regulation> Regulations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.ConfigClub();
            mb.ConfigPlayer();
            mb.ConfigMatch();
            mb.ConfigMatchResult();
            mb.ConfigStadium();
            mb.ConfigTournament();
            mb.ConfigStanding();
        }
    }
}