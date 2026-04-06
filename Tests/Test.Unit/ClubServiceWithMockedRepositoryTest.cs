using ApplicationBusinessRules;
using Model.EnterpriseBusinessRules;
using Model.Entities;
using Model.Repositories;
using Test.Repositories;

namespace Test
{
    [TestClass]
    public class ClubServiceWithMockedRepositoryTest
    {
        [TestMethod]
        public async Task Given_AValidClub_When_Create_Then_Should_CreateIt()
        {
            IClubRepository clubRepository = new ClubMockRepository();
            IStadiumRepository stadiumRepository = new StadiumMockRepository();
            var insertClub = new InsertClub(clubRepository);
            var getStadium = new GetStadium(stadiumRepository);
            var sut = new CreateClubUseCase(insertClub, getStadium);

            int clubId = await sut.ExecuteAsync(new Club
            {
                Name = "Argentinos Jr",
                Birthday = DateTime.Now,
                Email = "mail@mail.com",
                StadiumName = "Estadio Diego Armando Maradona"
            });

            Club clubCreated = await clubRepository.GetByIdAsync(clubId);
            Assert.IsNotNull(clubCreated);
        }

        [TestMethod]
        public async Task Given_AClubWithoutEmail_When_Create_Then_Should_NotCreateIt()
        {
            IClubRepository clubRepository = new ClubMockRepository();
            IStadiumRepository stadiumRepository = new StadiumMockRepository();
            var insertClub = new InsertClub(clubRepository);
            var getStadium = new GetStadium(stadiumRepository);
            var sut = new CreateClubUseCase(insertClub, getStadium);
            try
            {
                int clubId = await sut.ExecuteAsync(new Club
                {
                    Name = "Argentinos Jr",
                    Birthday = DateTime.Now,
                    StadiumName = "Estadio Diego Armando Maradona"
                });
                Assert.Fail();
            }
            catch (NullReferenceException)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }      
    }   
}