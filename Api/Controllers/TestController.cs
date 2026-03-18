using Microsoft.AspNetCore.Mvc;

namespace NetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Api Liga Libre");
        }
    }
}
