using Model.Entities;
using Model.Repositories;

namespace Test.Repositories
{
    internal class StadiumMockRepository : IStadiumRepository
    {
        private List<Stadium> _mock;

        public StadiumMockRepository()
        {
            this._mock = new List<Stadium>();
            this._mock.Add(new Stadium
            {
                Name = "Estadio Diego Armando Maradona",
                Capacity = 99354,
                ClubId = 1
            });
        }

        public async Task<Stadium> GetByIdAsync(string name)
        {
            return this._mock.First(x => x.Name == name);
        }
    }
}