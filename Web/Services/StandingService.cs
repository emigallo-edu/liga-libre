using System.Net.Http.Json;
using Web.Models;

namespace Web.Services;

public class StandingService
{
    private readonly HttpClient _http;

    public StandingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Standing>> GetByTournamentAsync(int tournamentId)
    {
        return await _http.GetFromJsonAsync<List<Standing>>($"Standing/TournamentId/{tournamentId}") ?? new();
    }
}
