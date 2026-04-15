using ApplicationBusinessRules;
using AutoMapper;
using Model.EnterpriseBusinessRules;
using Model.Entities;
using NetWebApi.Model;
using System.ComponentModel.DataAnnotations;
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

                cfg.CreateMap<ClubDTOV2, Club>()
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

        [TestMethod]
        public async Task Given_UnClienteV2QueEnviaContactName_When_SeCreaUnClubMediantePostClubV2_Then_ElClubQuedaPersistidoConElContactNameRecibido()
        {
            _stadiumRepository.Add(new Stadium { Name = "Monumental" });

            var dto = new ClubDTOV2
            {
                NombreClub = "River",
                Birthday = DateTime.Now.AddYears(-100),
                City = "BsAs",
                Email = "river@liga.com",
                NumberOfPartners = 100,
                Phone = "1234-5678",
                StadiumName = "Monumental",
                ContactName = "Juan Perez"
            };

            Assert.IsTrue(TryValidate(dto, out _),
                "El DTO con ContactName informado debería ser válido");

            var club = _mapper.Map<Club>(dto);
            await _crearClub.ExecuteAsync(club);

            var persistido = (await _clubRepository.GetAllAsync()).Single();
            Assert.AreEqual("River", persistido.Name);
            Assert.AreEqual("Juan Perez", persistido.ContactName);
        }

        [TestMethod]
        public async Task Given_UnClienteV2QueNoEnviaContactName_When_SeIntentaCrearUnClubMediantePostClubV2_Then_LaValidacionFallaYNoSePersisteNada()
        {
            _stadiumRepository.Add(new Stadium { Name = "Monumental" });

            var dto = new ClubDTOV2
            {
                NombreClub = "River",
                Birthday = DateTime.Now.AddYears(-100),
                City = "BsAs",
                Email = "river@liga.com",
                NumberOfPartners = 100,
                Phone = "1234-5678",
                StadiumName = "Monumental",
                ContactName = null!
            };

            var esValido = TryValidate(dto, out var errores);

            Assert.IsFalse(esValido, "El DTO sin ContactName no debería pasar la validación");
            Assert.IsTrue(errores.Any(e => e.MemberNames.Contains(nameof(ClubDTOV2.ContactName))),
                "El error de validación debe referirse a ContactName");
            Assert.AreEqual(0, (await _clubRepository.GetAllAsync()).Count,
                "Si la validación falla, no debe persistirse ningún club");
        }

        private static bool TryValidate(object dto, out List<ValidationResult> errores)
        {
            errores = new List<ValidationResult>();
            var context = new ValidationContext(dto);
            return Validator.TryValidateObject(dto, context, errores, validateAllProperties: true);
        }
    }
}
