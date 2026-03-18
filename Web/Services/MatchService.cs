using System.Net.Http.Json;
using Web.Models;

namespace Web.Services;

public class MatchService
{
    private readonly HttpClient _http;

    public MatchService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Match>> GetByTournamentAsync(int tournamentId)
    {
        return await _http.GetFromJsonAsync<List<Match>>($"Match/{tournamentId}") ?? new();
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegisterMatchDTO dto)
    {
        return await _http.PostAsJsonAsync("Match/Register", dto);
    }
}
