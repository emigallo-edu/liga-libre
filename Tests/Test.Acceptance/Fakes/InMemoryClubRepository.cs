using Model.Entities;
using Model.Repositories;

namespace Test.Acceptance.Fakes
{
    internal class InMemoryClubRepository : IClubRepository
    {
        private readonly List<Club> _clubs = new();
        private int _nextId = 1;

        public async Task<int> InsertAsync(Club club)
        {
            club.Id = _nextId++;
            _clubs.Add(club);
            return club.Id;
        }

        public async Task<List<Club>> GetAllAsync() => _clubs;

        public async Task<Club> GetByIdAsync(int id) => _clubs.First(x => x.Id == id);

        public Task ChangeName(int cludId, string newName) => throw new NotImplementedException();
        public Task<List<ShortClub>> GetAllShortAsync() => throw new NotImplementedException();
        public Task<List<Club>> GetClubsWithRegulations() => throw new NotImplementedException();
        public Task UpdateAsync(Club club) => throw new NotImplementedException();
        public Task UpdateWithStadiumAsync(List<Club> clubs) => throw new NotImplementedException();
    }
}
