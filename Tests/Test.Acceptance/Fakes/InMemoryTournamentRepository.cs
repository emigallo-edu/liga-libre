using Model.Entities;
using Model.Repositories;

namespace Test.Acceptance.Fakes
{
    internal class InMemoryTournamentRepository : ITournamentRepository
    {
        private readonly List<Tournament> _tournaments = new();
        private int _nextId = 1;

        public async Task<int> InsertAsync(Tournament item)
        {
            item.Id = _nextId++;
            _tournaments.Add(item);
            return item.Id;
        }

        public async Task<Tournament> GetByIdAsync(int id) => _tournaments.First(x => x.Id == id);

        public async Task<List<Tournament>> GetAllAsync() => _tournaments;
    }
}
