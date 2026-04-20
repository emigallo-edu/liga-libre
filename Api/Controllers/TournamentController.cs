using ApplicationBusinessRules;
using Microsoft.AspNetCore.Mvc;
using Model.EnterpriseBusinessRules;
using Model.Entities;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentController : Controller
    {
        private readonly CreateTournamentUseCase _createTournament;
        private readonly GetAllTournamentsUseCase _getAllTournaments;
        private readonly GetTournamentByIdUseCase _getTournamentById;
        private readonly CreateSingleLegTournamentUseCase _createSingleLegTournament;
        private readonly GetAllClubs _getAllClubs;

        public TournamentController(
            CreateTournamentUseCase createTournament,
            CreateSingleLegTournamentUseCase createSingleLegTournament,
            GetAllTournamentsUseCase getAllTournaments,
            GetTournamentByIdUseCase getTournamentById,
            GetAllClubs getAllClubs)
        {
            this._createTournament = createTournament;
            this._createSingleLegTournament = createSingleLegTournament;
            this._getAllTournaments = getAllTournaments;
            this._getTournamentById = getTournamentById;
            this._getAllClubs = getAllClubs;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                List<Club> clubs = await this._getAllClubs.ExecuteAsync();
                if (clubs.Count > 15)
                {
                    await this._createTournament.ExecuteAsync();
                }
                else
                {
                    await this._createSingleLegTournament.ExecuteAsync();
                }
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