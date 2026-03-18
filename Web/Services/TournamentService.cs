using System.Net.Http.Json;
using Web.Models;

namespace Web.Services;

public class TournamentService
{
    private readonly HttpClient _http;

    public TournamentService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Tournament>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Tournament>>("Tournament/all") ?? new();
    }

    public async Task<Tournament?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Tournament>($"Tournament/id/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync()
    {
        return await _http.PostAsync("Tournament", null);
    }
}
