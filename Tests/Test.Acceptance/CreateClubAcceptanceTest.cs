using ApplicationBusinessRules;
using AutoMapper;
using Model.EnterpriseBusinessRules;
using Model.Entities;
using NetWebApi.Model;
using Test.Acceptance.Fakes;

namespace Test.Acceptance
{
    [TestClass]
    public class CreateClubAcceptanceTest
    {
        private InMemoryClubRepository _clubRepository = null!;
        private InMemoryStadiumRepository _stadiumRepository = null!;
        private CreateClubUseCase _crearClub = null!;
        private IMapper _mapper = null!;

        [TestInitialize]
        public void Setup()
        {
            _clubRepository = new InMemoryClubRepository();
            _stadiumRepository = new InMemoryStadiumRepository();

            _crearClub = new CreateClubUseCase(
                new InsertClub(_clubRepository),
                new GetStadium(_stadiumRepository));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClubDTO, Club>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreClub))
                    .ReverseMap();
            });
            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task Given_UnClienteV1QueNoEnviaContactName_When_SeCreaUnClubMediantePostClub_Then_ElClubQuedaPersistidoConNombreTemporal()
        {
            _stadiumRepository.Add(new Stadium { Name = "Monumental" });

            var dto = new ClubDTO
            {
                NombreClub = "River",
                Birthday = DateTime.Now.AddYears(-100),
                City = "BsAs",
                Email = "river@liga.com",
                NumberOfPartners = 100,
                Phone = "1234-5678",
                StadiumName = "Monumental"
            };

            var club = _mapper.Map<Club>(dto);
            await _crearClub.ExecuteAsync(club);

            var persistido = (await _clubRepository.GetAllAsync()).Single();
            Assert.AreEqual("River", persistido.Name);
            Assert.AreEqual("nombre temporal", persistido.ContactName,
                "Los clientes v1 no envían ContactName, el servidor debe asignar el valor temporal");
        }
    }
}