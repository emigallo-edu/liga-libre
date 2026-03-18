using Microsoft.AspNetCore.Mvc;
using Model.Entities;
using Model.Repositories;
using Model.Services;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentController : Controller
    {
        private readonly CreateTournamentService _createTournamentService;
        private readonly ITournamentRepository _repository;

        public TournamentController(CreateTournamentService createTournamentService, ITournamentRepository repository)
        {
            this._createTournamentService = createTournamentService;
            this._repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            try
            {
                await this._createTournamentService.ExecuteAsync();
                return Ok($"Torneo {this._createTournamentService} creado correctamente");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await this._repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await this._repository.GetByIdAsync(id);
            return Ok(result);
        }
    }
}