using NetArchTest.Rules;
using Repository;

namespace Test.Architecture
{
    [TestClass]
    public class Repository
    {
        [TestMethod]
        public void Given_Repository_When_CheckingDependencies_Then_ShouldNotDependOnApi()
        {
            var result = Types.InAssembly(typeof(ClubFileRepository).Assembly)
                .ShouldNot()
                .HaveDependencyOn("NetWebApi")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Repository no debe depender de Api");
        }

        [TestMethod]
        public void Given_Repository_When_CheckingDependencies_Then_ShouldNotDependOnApplicationBusinessRules()
        {
            var result = Types.InAssembly(typeof(ClubFileRepository).Assembly)
                .ShouldNot()
                .HaveDependencyOn("ApplicationBusinessRules")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Repository no debe depender de ApplicationBusinessRules");
        }
    }
}