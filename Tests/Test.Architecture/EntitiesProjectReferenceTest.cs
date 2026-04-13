using NetArchTest.Rules;

namespace Test.Architecture
{
    [TestClass]
    public sealed class EntitiesProjectReferenceTest
    {
        [TestMethod]
        public void Given_Entities_When_CheckingDependencies_Then_ShouldNotDependOnApi()
        {
            var result = Types.InAssembly(typeof(Model.Entities.Club).Assembly)
                .ShouldNot()
                .HaveDependencyOn("NetWebApi")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Entities no debe depender de Api");
        }

        [TestMethod]
        public void Given_Entities_When_CheckingDependencies_Then_ShouldNotDependOnApplicationBusinessRules()
        {
            var result = Types.InAssembly(typeof(Model.Entities.Club).Assembly)
                .ShouldNot()
                .HaveDependencyOn("ApplicationBusinessRules")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Entities no debe depender de ApplicationBusinessRules");
        }

        [TestMethod]
        public void Given_Entities_When_CheckingDependencies_Then_ShouldNotDependOnRepository()
        {
            var result = Types.InAssembly(typeof(Model.Entities.Club).Assembly)
                .ShouldNot()
                .HaveDependencyOn("Repository")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful, "Entities no debe depender de Repository");
        }    
    }
}