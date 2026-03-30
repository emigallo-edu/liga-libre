using ApplicationBusinessRules;
using Microsoft.AspNetCore.Mvc;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseAuditController : Controller
    {
        private readonly GetAllResponseAuditsUseCase _getAllResponseAudits;

        public ResponseAuditController(GetAllResponseAuditsUseCase getAllResponseAudits)
        {
            this._getAllResponseAudits = getAllResponseAudits;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this._getAllResponseAudits.ExecuteAsync();
            return Ok(result);
        }
    }
}
