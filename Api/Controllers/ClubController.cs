using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Model.Repositories;
using NetWebApi.Model;
using NetWebApi.Utils;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ClubController : ControllerBase
    {
        private readonly IClubRepository _repository;
        private readonly IMapper _mapper;

        public ClubController(IClubRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClubDTO club)
        {
            await this._repository.InsertAsync(this._mapper.Map<Club>(club));
            return Ok();
        }


        [HttpGet("id/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            Club result = await this._repository.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this._repository.GetAllAsync();

            foreach (var item in result.Where(x => x.Stadium != null))
            {
                item.Stadium.Club = null;
            }

            return Ok(result);
        }

        [HttpGet("short")]
        public async Task<IActionResult> GetAllShort()
        {
            var result = await this._repository.GetAllShortAsync();
            return Ok(result);
        }

        [HttpPatch("name")]
        public async Task<IActionResult> ChangeName(ChangeClubNameDTO dto)
        {
            await this._repository.ChangeName(dto.Id, dto.Name);
            return Ok("El registro se modifico correctamente");
        }

        [HttpPut()]
        public async Task<IActionResult> Update(ClubDTO club)
        {
            await this._repository.UpdateAsync(this._mapper.Map<Club>(club));
            return Ok();
        }

        [HttpPut("withStadium")]
        public async Task<IActionResult> UpdateWithStadium(List<Club> clubs)
        {
            await this._repository.UpdateWithStadiumAsync(clubs);
            return Ok();
        }

        [HttpGet("Test")]
        public IActionResult GetClubs()
        {
            List<Club> clubs = new List<Club>();
            clubs.Add(new Club
            {
                Name = "Club1",
                Birthday = new DateTime(1994, 2, 1),
                City = "Mendoza"
            });
            clubs.Add(new Club
            {
                Name = "Club2",
                Birthday = new DateTime(1992, 2, 1),
                City = "Córdoba"
            });
            clubs.Add(new Club
            {
                Name = "Club3",
                Birthday = new DateTime(1963, 2, 1),
                City = "Misiones"
            });
            clubs.Add(new Club
            {
                Name = "Club4",
                Birthday = new DateTime(1987, 2, 1),
                City = "Neuquen"
            });

            List<Club> result = clubs
                .Where(x => x.Birthday.Year > 1993)
                .ToList();

            List<Club> result1 = clubs
                .Where(Filter)
                .ToList();

            List<Club> result2 = clubs
               .Where(x => x.IsFromBuenosAires())
               .ToList();

            List<Club> result3 = clubs
             .WhereExtension(x => x.IsFromBuenosAiresExtensions("Nombre"))
             .ToList();

            return Ok(result);
        }

        [HttpGet("regulations")]
        public async Task<IActionResult> GetClubsWithRegulations()
        {
            var list = await this._repository.GetClubsWithRegulations();
            return Ok(list);
        }

        private bool Filter(Club club)
        {
            return club.Birthday.Year > 1993;
        }

        private List<Club> FilterOldSchool(List<Club> clubs)
        {
            List<Club> result = new List<Club>();

            foreach (Club club in clubs)
            {
                if (club.Birthday.Year > 1993)
                {
                    result.Add(club);
                }
            }

            return result;
        }

        private List<T> Where<T>(List<T> clubs, Func<T, bool> filter)
        {
            List<T> result = new List<T>();

            foreach (T club in clubs)
            {
                if (filter(club))
                {
                    result.Add(club);
                }
            }

            return result;
        }
    }
}