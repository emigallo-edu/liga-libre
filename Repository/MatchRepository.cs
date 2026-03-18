using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Repository
{
    public class MatchRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public MatchRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }

        public async Task<Match> GetAsync(int id)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                return await context.Matches
                    .FirstAsync(x => x.Id == id);
            }
        }

        /// <summary>
        /// Consulta simple por método de extensión
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        public async Task<List<Match>> GetByTournamentAsyncV1(int? tournamentId, bool sortAscending = true)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                IQueryable<Match> query = context.Matches;

                if (tournamentId != null)
                {
                    query = query.Where(x => x.TournamentId == tournamentId);
                }

                if (sortAscending)
                {
                    query = query.OrderBy(x => x.Date);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Date);
                }

                return await query.ToListAsync();
            }
        }

        /// <summary>
        /// Misma consulta que V1 pero por Linq
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        public async Task<List<Match>> GetByTournamentAsyncV2(int tournamentId)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                IQueryable<Match> query = from m in context.Matches
                                          where m.TournamentId == tournamentId
                                          select m;

                return await query.ToListAsync();
            }
        }

        /// <summary>
        /// Consulta con joins por método de extensión
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        public async Task<List<Match>> GetByTournamentAsyncV3(int tournamentId)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                return await context.Matches
                        .Include(x => x.LocalClub)
                        .Include(x => x.VisitingClub)
                        .Where(x => x.TournamentId == tournamentId)
                        .OrderBy(x => x.Date)
                        .ToListAsync();
            }
        }

        /// <summary>
        /// Misma consulta que V3 pero por Linq
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        public async Task<List<Match>> GetByTournamentAsyncV4(int tournamentId)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                var query = from m in context.Matches
                            join localClub in context.Clubs
                                on m.LocalClubId equals localClub.Id
                            join visitingClub in context.Clubs
                                on m.VisitingClubId equals visitingClub.Id
                            where m.TournamentId == tournamentId
                            select new Match()
                            {
                                Id = m.Id,
                                VisitingClubId = visitingClub.Id,
                                TournamentId = tournamentId,
                                LocalClubId = localClub.Id,
                                Date = DateTime.Now,
                                LocalClub = localClub,
                                VisitingClub = visitingClub
                            };

                return await query.ToListAsync();
            }
        }

        public async Task<int> InsertMatchResult(MatchResult item)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                context.MatchsResults.Add(item);
                return await context.SaveChangesAsync();
            }
        }
    }
}