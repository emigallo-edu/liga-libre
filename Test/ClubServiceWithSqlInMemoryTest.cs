using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Repository;

namespace Test
{
    [TestClass]
    public class ClubServiceWithSqlInMemoryTest
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        public ClubServiceWithSqlInMemoryTest()
        {
            this._connection = new SqliteConnection("Filename=:memory:");
            this._connection.Open();
            this._contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlite(this._connection)
               .Options;
        }

        [TestMethod]
        public async Task Given_AClubId_When_GetById_Then_Should_GetIt()
        {
            ApplicationDbContext context = new ApplicationDbContext(this._contextOptions);
            Club club = new Club()
            {
                Name = "Argentinos Jr",
                Birthday = new DateTime(1901, 5, 25),
                Email = "mail@mail.com",
                //StadiumName = "Estadio Diego Armando Maradona",
                City = "Bs As",
                Phone = "1234567890",
                NumberOfPartners = 1231,
                Address = "Av. Rivadavia 1234"
            };
            if (context.Database.EnsureCreated())
            {
                context.Clubs.Add(club);
                await context.SaveChangesAsync();
            }
            ClubDbRepository sut = new ClubDbRepository(this._contextOptions);

            Club clubCreated = await sut.GetByIdAsync(club.Id);

            Assert.IsNotNull(clubCreated);
        }
    }
}