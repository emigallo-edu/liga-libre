using ApplicationBusinessRules;
using Microsoft.AspNetCore.Mvc;
using NetWebApi.Model;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : Controller
    {
        private readonly GetMatchesByTournamentUseCase _getMatchesByTournament;
        private readonly RegisterMatchResultUseCase _registerMatchResult;

        public MatchController(
            GetMatchesByTournamentUseCase getMatchesByTournament,
            RegisterMatchResultUseCase registerMatchResult)
        {
            this._getMatchesByTournament = getMatchesByTournament;
            this._registerMatchResult = registerMatchResult;
        }

        [HttpGet("{tournamentId}")]
        public async Task<IActionResult> GetAll([FromRoute] int tournamentId)
        {
            var result = await this._getMatchesByTournament.ExecuteAsyncV1(tournamentId);
            return Ok(result);
        }

        [HttpGet("v2/{tournamentId}")]
        public async Task<IActionResult> GetAllV2([FromRoute] int tournamentId)
        {
            var result = await this._getMatchesByTournament.ExecuteAsyncV2(tournamentId);
            return Ok(result);
        }

        [HttpGet("v3/{tournamentId}")]
        public async Task<IActionResult> GetAllV3([FromRoute] int tournamentId)
        {
            var result = await this._getMatchesByTournament.ExecuteAsyncV3(tournamentId);
            return Ok(result);
        }

        [HttpGet("v4/{tournamentId}")]
        public async Task<IActionResult> GetAllV4([FromRoute] int tournamentId)
        {
            var result = await this._getMatchesByTournament.ExecuteAsyncV4(tournamentId);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterMatch(RegisterMatchDTO item)
        {
            await this._registerMatchResult.ExecuteAsync(
                item.StandingId,
                item.Matchid,
                item.LocalClubGoals,
                item.VisitingClubGoals);
            return Ok();
        }
    }
}