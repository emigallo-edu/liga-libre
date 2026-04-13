using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Repository;

namespace Api.Controllers
{
    public class DeploymentController : Controller
    {
        private readonly MigrationRepository _migrationRepository;

        public DeploymentController(MigrationRepository migrationRepository)
        {
            _migrationRepository = migrationRepository;
        }

        [HttpGet("update-database")]
        public async Task<IActionResult> UpdateDatabase()
        {
            IEnumerable<string> applied = await _migrationRepository.MigrateAsync();
            IEnumerable<string> pending = await _migrationRepository.GetPendingMigrations();
            string message = $"Migraciones aplicadas: {applied.Count()} - pendientes {pending.Count()}";
            return Ok(message);
        }
    }
}