using ApplicationBusinessRules;
using Microsoft.AspNetCore.Mvc;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentController : Controller
    {
        private readonly CreateTournamentUseCase _createTournament;
        private readonly GetAllTournamentsUseCase _getAllTournaments;
        private readonly GetTournamentByIdUseCase _getTournamentById;

        public TournamentController(
            CreateTournamentUseCase createTournament,
            GetAllTournamentsUseCase getAllTournaments,
            GetTournamentByIdUseCase getTournamentById)
        {
            this._createTournament = createTournament;
            this._getAllTournaments = getAllTournaments;
            this._getTournamentById = getTournamentById;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                await this._createTournament.ExecuteAsync();
                return Ok($"Torneo {this._createTournament} creado correctamente");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this._getAllTournaments.ExecuteAsync();
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await this._getTournamentById.ExecuteAsync(id);
            return Ok(result);
        }
    }
}
