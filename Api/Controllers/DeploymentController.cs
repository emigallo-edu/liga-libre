using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeploymentController : Controller
    {
        private readonly MigrationRepository _migrationRepository;

        public DeploymentController(MigrationRepository migrationRepository)
        {
            _migrationRepository = migrationRepository;
        }

        [HttpPost("update-database")]
        public async Task<IActionResult> UpdateDatabase()
        {
            IEnumerable<string> applied = await _migrationRepository.MigrateAsync();
            IEnumerable<string> pending = await _migrationRepository.GetPendingMigrations();
            string message = $"Migraciones aplicadas: {applied.Count()} - pendientes {pending.Count()}";
            return Ok(message);
        }

        [HttpGet("migrations")]
        public async Task<IActionResult> GetMigrations()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Migraciones aplicadas:");
            foreach (string applied in await _migrationRepository.GetAppliedMigrationsAsync())
            {
                builder.AppendLine($"- {applied}");
            }
            return Ok(builder.ToString());
        }

        [HttpGet("build")]
        public IActionResult Index()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
            return Ok($"Api Liga Libre v{version}");
        }
    }
}