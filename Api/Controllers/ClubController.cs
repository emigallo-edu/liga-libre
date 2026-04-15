using ApplicationBusinessRules;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using NetWebApi.Model;
using NetWebApi.Utils;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ClubController : ControllerBase
    {
        private readonly CreateClubUseCase _createClub;
        private readonly GetClubByIdUseCase _getClubById;
        private readonly GetAllClubsUseCase _getAllClubs;
        private readonly GetAllClubsShortUseCase _getAllClubsShort;
        private readonly ChangeClubNameUseCase _changeClubName;
        private readonly UpdateClubUseCase _updateClub;
        private readonly UpdateClubWithStadiumUseCase _updateClubWithStadium;
        private readonly GetClubsWithRegulationsUseCase _getClubsWithRegulations;
        private readonly IMapper _mapper;

        public ClubController(
            CreateClubUseCase createClub,
            GetClubByIdUseCase getClubById,
            GetAllClubsUseCase getAllClubs,
            GetAllClubsShortUseCase getAllClubsShort,
            ChangeClubNameUseCase changeClubName,
            UpdateClubUseCase updateClub,
            UpdateClubWithStadiumUseCase updateClubWithStadium,
            GetClubsWithRegulationsUseCase getClubsWithRegulations,
            IMapper mapper)
        {
            this._createClub = createClub;
            this._getClubById = getClubById;
            this._getAllClubs = getAllClubs;
            this._getAllClubsShort = getAllClubsShort;
            this._changeClubName = changeClubName;
            this._updateClub = updateClub;
            this._updateClubWithStadium = updateClubWithStadium;
            this._getClubsWithRegulations = getClubsWithRegulations;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubDTO club)
        {
            await this._createClub.ExecuteAsync(this._mapper.Map<Club>(club));
            return Ok();
        }


        [HttpGet("id/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            Club result = await this._getClubById.ExecuteAsync(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this._getAllClubs.ExecuteAsync();
            return Ok(result);
        }

        [HttpGet("short")]
        public async Task<IActionResult> GetAllShort()
        {
            var result = await this._getAllClubsShort.ExecuteAsync();
            return Ok(result);
        }

        [HttpPatch("name")]
        public async Task<IActionResult> ChangeName(ChangeClubNameDTO dto)
        {
            await this._changeClubName.ExecuteAsync(dto.Id, dto.Name);
            return Ok("El registro se modifico correctamente");
        }

        [HttpPut()]
        public async Task<IActionResult> Update(ClubDTO club)
        {
            await this._updateClub.ExecuteAsync(this._mapper.Map<Club>(club));
            return Ok();
        }

        [HttpPut("withStadium")]
        public async Task<IActionResult> UpdateWithStadium(List<Club> clubs)
        {
            await this._updateClubWithStadium.ExecuteAsync(clubs);
            return Ok();
        }

        [HttpGet("regulations")]
        public async Task<IActionResult> GetClubsWithRegulations()
        {
            var list = await this._getClubsWithRegulations.ExecuteAsync();
            return Ok(list);
        }

        private bool Filter(Club club)
        {
            return club.Birthday.Year > 1993;
        }
    }
}