using Model.Entities;
using Model.Repositories;

namespace Test.Acceptance.Fakes
{
    internal class InMemoryStadiumRepository : IStadiumRepository
    {
        private readonly List<Stadium> _stadiums = new();

        public void Add(Stadium stadium) => _stadiums.Add(stadium);

        public async Task<Stadium> GetByIdAsync(string name)
            => _stadiums.FirstOrDefault(x => x.Name == name)!;
    }
}
