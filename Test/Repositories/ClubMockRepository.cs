using Model.Entities;
using Model.Repositories;

namespace Test.Repositories
{
    internal class ClubMockRepository : IClubRepository
    {
        private List<Club> _mock;

        public ClubMockRepository()
        {
            this._mock = new List<Club>();
        }

        public async Task ChangeName(int cludId, string newName)
        {
            this._mock.First(x => x.Id == cludId).Name = newName;
        }

        public async Task<List<Club>> GetAllAsync()
        {
            return this._mock;
        }

        public async Task<List<ShortClub>> GetAllShortAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return this._mock.First(x => x.Id == id);
        }

        public Task<List<Club>> GetClubsWithRegulations()
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(Club club)
        {
            club.Id = 379;
            this._mock.Add(club);
            return club.Id;
        }

        public async Task UpdateAsync(Club club)
        {
            Club existing = this._mock.First(x => x.Id == club.Id);
            existing.Name = club.Name;
            existing.Birthday = club.Birthday;
            existing.City = club.City;
            existing.Email = club.Email;
            existing.NumberOfPartners = club.NumberOfPartners;
            existing.Phone = club.Phone;
            existing.Address = club.Address;
            existing.StadiumName = club.StadiumName;
        }

        public async Task UpdateWithStadiumAsync(List<Club> clubs)
        {
            throw new NotImplementedException();
        }
    }
}