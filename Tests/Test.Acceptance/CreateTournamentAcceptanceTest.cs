using ApplicationBusinessRules;
using Model.Entities;
using Model.EnterpriseBusinessRules;
using Model.Repositories;
using Test.Acceptance.Fakes;

namespace Test.Acceptance
{
    [TestClass]
    public class CreateTournamentAcceptanceTest
    {
        private IClubRepository _clubRepository = null!;
        private ITournamentRepository _tournamentRepository = null!;
        private CreateTournamentUseCase _crearTorneo = null!;
        private GetAllTournamentsUseCase _listarTorneos = null!;

        [TestInitialize]
        public void Setup()
        {
            _clubRepository = new InMemoryClubRepository();
            _tournamentRepository = new InMemoryTournamentRepository();

            _crearTorneo = new CreateTournamentUseCase(
                new GetAllClubs(_clubRepository),
                new InsertTournament(_tournamentRepository));

            _listarTorneos = new GetAllTournamentsUseCase(
                new GetAllTournaments(_tournamentRepository));
        }

        [TestMethod]
        public async Task Given_UnaLigaConClubesInscriptos_When_ElOrganizadorCreaUnTorneo_Then_ElTorneoQuedaDisponibleParaLosParticipantes()
        {
            // Given: una liga con clubes ya inscriptos
            await InscribirClubesAsync("Argentinos Jr", "River", "Boca", "San Lorenzo");

            // When: el organizador dispara la creación del torneo
            await _crearTorneo.ExecuteAsync();

            // Then: el torneo aparece publicado, con fixture y participantes,
            var torneos = await _listarTorneos.ExecuteAsync();
            Assert.AreEqual(1, torneos.Count, "Debería existir un torneo publicado");

            var torneo = torneos.Single();
            Assert.IsTrue(torneo.Matches.Any(),
                "El torneo publicado debe incluir el fixture de partidos");
            Assert.IsTrue(torneo.Standings.Any(),
                "El torneo publicado debe incluir la tabla de posiciones inicial");
            Assert.IsTrue(torneo.Start <= torneo.End,
                "El torneo debe tener un período de disputa válido");
        }

        private async Task InscribirClubesAsync(params string[] nombres)
        {
            foreach (var nombre in nombres)
            {
                await _clubRepository.InsertAsync(new Club
                {
                    Name = nombre,
                    Email = $"{nombre.ToLower().Replace(" ", "")}@liga.com",
                    Birthday = DateTime.Now,
                    StadiumName = $"Estadio {nombre}"
                });
            }
        }
    }
}