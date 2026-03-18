using Model.Entities;
using Model.Repositories;

namespace Model.Services
{
    public class GetAllClubsService
    {
        private readonly IClubRepository _clubRepository;

        public GetAllClubsService(IClubRepository clubRepository)
        {
            this._clubRepository = clubRepository;
        }

        public async Task<List<Club>> ExecuteAsync()
        {
            return await this._clubRepository.GetAllAsync();
        }
    }
}