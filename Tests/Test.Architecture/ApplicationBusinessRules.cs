using ApplicationBusinessRules;
using NetArchTest.Rules;

namespace Test.Architecture
{
    [TestClass]
    public class ApplicationBusinessRules
    {
        [TestMethod]
        public void Given_ApplicationBusinessRules_When_CheckingDependencies_Then_ShouldNotDependOnApi()
        {
            var result = Types.InAssembly(typeof(GetAllClubsUseCase).Assembly)
                .ShouldNot()
                .HaveDependencyOn("NetWebApi")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "ApplicationBusinessRules no debe depender de Api");
        }

        [TestMethod]
        public void Given_ApplicationBusinessRules_When_CheckingDependencies_Then_ShouldNotDependOnRepository()
        {
            var result = Types.InAssembly(typeof(GetAllClubsUseCase).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Repository")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "ApplicationBusinessRules no debe depender de Repository");
        }

        [TestMethod]
        public void Given_ApplicationBusinessRules_When_CheckingDependencies_Then_ShouldNotDependOnSecurity()
        {
            var result = Types.InAssembly(typeof(GetAllClubsUseCase).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Security")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "ApplicationBusinessRules no debe depender de Security");
        }
    }
}