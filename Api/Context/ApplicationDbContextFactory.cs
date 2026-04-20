using ApplicationBusinessRules;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Model.EnterpriseBusinessRules;
using Model.Repositories;
using Repository;
using Security;

namespace NetWebApi.Context
{
    public enum DatabaseType
    {
        SqlServer,
        Files
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            contextOptionsBuilder.Config();
            return new ApplicationDbContext(contextOptionsBuilder.Options);
        }
    }

    public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
    {
        public SecurityDbContext CreateDbContext(string[] args)
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<SecurityDbContext>();
            contextOptionsBuilder.Config();
            return new SecurityDbContext(contextOptionsBuilder.Options);
        }
    }

    public static class ApplicationDbContextFactoryConfig
    {
        private static IServiceProvider _provider;

        public static void AddApplicationDbContext(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.Config());
        }

        public static void AddInMemoryApplicationDbContext(this IServiceCollection services)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddSingleton(connection);
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var conn = sp.GetRequiredService<SqliteConnection>();
                options
                    .UseSqlite(conn)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(Console.WriteLine, LogLevel.Trace, DbContextLoggerOptions.SingleLine);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();

            DataSeeder.SeedLigaLibreData(out var clubes, out var torneo, out var partidos, out var resultados);
            db.Clubs.AddRange(clubes);
            db.Tournaments.Add(torneo);
            db.Matches.AddRange(partidos);
            db.AddRange(resultados);
            db.SaveChanges();
        }

        public static void AddSecurityDbContext(this IServiceCollection services)
        {
            services.AddDbContext<SecurityDbContext>(
                options => options.Config());
        }

        public static void AddRepositories(this IServiceCollection services, DatabaseType databaseType)
        {
            services.AddApplicationDbContext();
            if (databaseType == DatabaseType.Files)
            {
                services.AddScoped<IClubRepository, ClubFileRepository>(
                    x => new ClubFileRepository(Path.Combine(Environment.CurrentDirectory, "Files")));
            }
            else
            {
                services.AddScoped<IClubRepository, ClubDbRepository>();
            }
            services.AddScoped<IResponseAuditRepository, ResponseAuditRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IStandingRepository, StandingRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<IStadiumRepository, StadiumRepository>();
            services.AddScoped<MigrationRepository>();
        }

        public static void AddEnterpriseBusinessRules(this IServiceCollection services)
        {
            services.AddScoped<GetClubById>();
            services.AddScoped<GetAllClubs>();
            services.AddScoped<GetAllClubsShort>();
            services.AddScoped<InsertClub>();
            services.AddScoped<ChangeClubName>();
            services.AddScoped<UpdateClub>();
            services.AddScoped<UpdateClubWithStadium>();
            services.AddScoped<GetClubsWithRegulations>();
            services.AddScoped<InsertTournament>();
            services.AddScoped<GetTournamentById>();
            services.AddScoped<GetAllTournaments>();
            services.AddScoped<GetMatch>();
            services.AddScoped<GetMatchesByTournament>();
            services.AddScoped<InsertMatchResult>();
            services.AddScoped<GetStandingsByTournament>();
            services.AddScoped<GetStanding>();
            services.AddScoped<UpdateStanding>();
            services.AddScoped<GetStadium>();
            services.AddScoped<GetAllResponseAudits>();
            services.AddScoped<InsertResponseAudit>();
        }

        public static void AddApplicationBusinessRules(this IServiceCollection services)
        {
            services.AddScoped<CreateTournamentUseCase>();
            services.AddScoped<CreateSingleLegTournamentUseCase>();
            services.AddScoped<CreateClubUseCase>();
            services.AddScoped<GetClubByIdUseCase>();
            services.AddScoped<GetAllClubsUseCase>();
            services.AddScoped<GetAllClubsShortUseCase>();
            services.AddScoped<ChangeClubNameUseCase>();
            services.AddScoped<UpdateClubUseCase>();
            services.AddScoped<UpdateClubWithStadiumUseCase>();
            services.AddScoped<GetClubsWithRegulationsUseCase>();
            services.AddScoped<GetAllTournamentsUseCase>();
            services.AddScoped<GetTournamentByIdUseCase>();
            services.AddScoped<GetMatchesByTournamentUseCase>();
            services.AddScoped<RegisterMatchResultUseCase>();
            services.AddScoped<GetStandingsByTournamentUseCase>();
            services.AddScoped<GetAllResponseAuditsUseCase>();
        }

        public static T Get<T>()
        {
            return _provider.GetRequiredService<T>();
        }

        public static void Config(this DbContextOptionsBuilder contextOptionsBuilder)
        {
            string connectionString = GetConnectionString();
            contextOptionsBuilder.UseSqlServer(connectionString, x =>
                x.MigrationsHistoryTable("_MigrationsHistory", "dbo")
                .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
        }

        public static void SetProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public static IServiceProvider GetProvider()
        {
            return _provider;
        }

        private static string GetConnectionString()
        {
            string? cs = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (cs is null)
            {
                throw new ArgumentException("No se encontró la variable de entorno CONNECTION_STRING");
            }
            return cs;
        }
    }
}