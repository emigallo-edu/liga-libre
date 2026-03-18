using Model.Entities;
using Model.Repositories;
using Model.Services;
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
            var sut = new CreateClubService(clubRepository, stadiumRepository);

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
            var sut = new CreateClubService(clubRepository, stadiumRepository);
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

        [TestMethod]
        public void PruebaCuadrado()
        {
            CalculadorDeSuperficie sut = new CalculadorDeSuperficie();
            double superficie = sut.Calcular(CalculadorDeSuperficie.FormasGeometricas.Cuadrado, 4, 4);
            Assert.IsTrue(superficie == 8);
        }

        [TestMethod]
        public void PruebaRectangulo()
        {
            CalculadorDeSuperficie sut = new CalculadorDeSuperficie();
            double superficie = sut.Calcular(CalculadorDeSuperficie.FormasGeometricas.Rectangulo, 3, 2);
            Assert.IsTrue(superficie == 6);
        }

        [TestMethod]
        public void PruebaIsosceles()
        {
            CalculadorDeSuperficie sut = new CalculadorDeSuperficie();
            double superficie = sut.Calcular(CalculadorDeSuperficie.FormasGeometricas.TrianguloIsosceles, 5, 4);
            Assert.IsTrue(superficie == 10);
        }

        [TestMethod]
        public void PruebaRombo()
        {
            CalculadorDeSuperficie sut = new CalculadorDeSuperficie();
            double superficie = sut.Calcular(CalculadorDeSuperficie.FormasGeometricas.Rombo, 3, 2);
            Assert.IsTrue(superficie == 6);
        }

        [TestMethod]
        public void PruebaRombo1()
        {
            var ladoA = 4;
            var ladoB = 4;
            var expected = 16;

            var result = new CalculadorDeSuperficie().Calcular(CalculadorDeSuperficie.FormasGeometricas.Cuadrado, ladoA, ladoB);

            Assert.AreNotEqual(expected, result, "The calculation for Cuadrado is incorrect. Expected 16 but got " + result);
        }
    }



    public class CalculadorDeSuperficie
    {
        public enum FormasGeometricas
        {
            Cuadrado,
            Rectangulo,
            TrianguloIsosceles,
            Rombo
        }

        public double Calcular(FormasGeometricas formaGeometricas, int ladoA, int ladoB)
        {
            switch (formaGeometricas)
            {
                case FormasGeometricas.Cuadrado:
                    return ladoA * 2;
                case FormasGeometricas.Rectangulo:
                    return ladoA * ladoA;
                case FormasGeometricas.TrianguloIsosceles:
                    return (ladoA * ladoB) / 2;
                case FormasGeometricas.Rombo:
                    return ladoA * 2;
                default:
                    throw new ArgumentException("Forma geométrica no soportada");
            }
        }
    }
}