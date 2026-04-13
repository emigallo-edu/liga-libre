using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Repositories;

namespace Repository
{
    public class ClubDbRepository : IClubRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ClubDbRepository(DbContextOptions<ApplicationDbContext> options)
        {
            this._options = options;
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                return await context.Clubs
                    .FirstAsync(x => x.Id == id);
            }
        }

        public async Task<List<Club>> GetAllAsync()
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                return await context.Clubs
                    .Include(x => x.Stadium)
                    .ToListAsync();
            }
        }

        public async Task<List<Club>> GetAllSelectingAddressAndCityAsync()
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                var result = from club in context.Clubs
                             select new Club
                             {
                                 Address = club.Address,
                                 City = club.City,
                             };

                return await result.ToListAsync();
            }
        }

        public async Task<int> InsertAsync(Club club)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                context.Add(club);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<List<ShortClub>> GetAllShortAsync()
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                var result = from club in context.Clubs
                             select new ShortClub
                             {
                                 Address = club.Address,
                                 City = club.City,
                                 Phone = club.Phone
                             };

                return await result.ToListAsync();
            }
        }

        public async Task<List<ShortClub>> GetAllShortAsyncC()
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                var query = from club in context.Clubs
                            join stadium in context.Stadiums
                                on club.Id equals stadium.Capacity
                            select new
                            {
                                Address = club.Address,
                                City = club.City,
                                Phone = club.Phone,
                                Capacity = stadium.Capacity
                            };

                List<ShortClub> result = new List<ShortClub>();
                foreach (var item in query)
                {
                    result.Add(new ShortClub()
                    {
                        Address = item.Address,
                        City = item.City,
                        Phone = item.Phone
                    });
                }
                return result;
            }
        }


        /// <summary>
        /// Hacemos un update de un Club que es una instancia diferente de la del DbContext
        /// </summary>
        /// <param name="cludId"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Club clubDTO)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                Club existingClub = await context.Clubs
                    //.AsNoTracking()
                    .FirstAsync(x => x.Id == clubDTO.Id);

                clubDTO.Name = existingClub.Name;
                context.Clubs.Update(clubDTO);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Actualziamos 2 entidades directo con el DbContext
        /// </summary>
        /// <param name="clubs"></param>
        /// <returns></returns>
        public async Task UpdateWithStadiumAsync(List<Club> clubs)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                List<Club> existingClubs = await context.Clubs.AsNoTracking()
                    .Include(x => x.Stadium)
                    .Where(x => clubs.Select(clubs => clubs.Id).Contains(x.Id))
                    .ToListAsync();

                foreach (Club club in clubs)
                {
                    if (existingClubs.Any(x => x.Id == club.Id))
                    {
                        context.Clubs.Update(club);
                    }
                    else
                    {
                        context.Clubs.Add(club);
                    }

                    if (existingClubs.FirstOrDefault()?.Stadium?.Name == club?.Stadium?.Name)
                    {
                        context.Stadiums.Update(club.Stadium);
                    }
                    else
                    {
                        context.Stadiums.Add(club.Stadium);
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Actualziamos 2 entidades usando un DTO
        /// </summary>
        /// <param name="clubs"></param>
        /// <returns></returns>
        public async Task UpdateWithStadiumAsync1(List<Club> clubs)
        {
            List<Club> clubsToPersist = new List<Club>();
            List<Stadium> stadiumsToPersist = new List<Stadium>();

            using (ApplicationDbContext context = new ApplicationDbContext(this._options))
            {
                foreach (Club club in clubs)
                {
                    if (!clubsToPersist.Any(x => x.Id == club.Id))
                    {
                        clubsToPersist.Add(club.GetDTO());
                    }
                    else
                    {
                        var existing = clubsToPersist.First(x => x.Id == club.Id);
                        existing.NumberOfPartners++;
                    }

                    if (!stadiumsToPersist.Any(x => x.Name == club.StadiumName))
                    {
                        stadiumsToPersist.Add(club.Stadium);
                    }
                    else
                    {
                        var existing = stadiumsToPersist.First(x => x.Name == club.StadiumName);
                        existing.Capacity++;
                    }
                }

                foreach (Club c in clubsToPersist)
                {
                    if (context.Clubs.Any(x => x.Id == c.Id))
                    {
                        context.Clubs.Update(c);
                    }
                    else
                    {
                        context.Clubs.Add(c);
                    }
                }

                foreach (Stadium s in stadiumsToPersist)
                {
                    if (context.Stadiums.Any(x => x.Name == s.Name))
                    {
                        context.Stadiums.Update(s);
                    }
                    else
                    {
                        context.Stadiums.Add(s);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Modificamos una property de una entidad ya existente
        /// Inspeccionamos la variable ChangeTracker del DbContext
        /// </summary>
        /// <param name="cludId"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public async Task ChangeName(int cludId, string newName)
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                string initialTrack = context.ChangeTracker.DebugView.LongView;
                Club club = context.Clubs
                    //.AsNoTracking()
                    .First(x => x.Id == cludId);
                string getTrack = context.ChangeTracker.DebugView.LongView;
                club.Name = newName;

                context.Update(club);
                string changeNameTrack = context.ChangeTracker.DebugView.LongView;
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Club>> GetClubsWithRegulations()
        {
            using (var context = new ApplicationDbContext(this._options))
            {
                var query = from c in context.Clubs
                            join s in context.Stadiums
                                on c.Id equals s.ClubId
                            join r in context.Regulations
                                on s.Aux equals r.Id.ToString()
                            where s.Capacity > 1000
                            select c;
                return await query.ToListAsync();
            }
        }
    }
}