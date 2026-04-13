using System.Reflection;
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
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            return Ok($"Api Liga Libre v{version}");
        }
    }
}
