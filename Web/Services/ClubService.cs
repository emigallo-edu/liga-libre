using System.Net.Http.Json;
using Web.Models;

namespace Web.Services;

public class ClubService
{
    private readonly HttpClient _http;

    public ClubService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Club>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Club>>("Club/all") ?? new();
    }

    public async Task<Club?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Club>($"Club/id/{id}");
    }

    public async Task<HttpResponseMessage> CreateAsync(ClubDTO club)
    {
        return await _http.PostAsJsonAsync("Club", club);
    }

    public async Task<HttpResponseMessage> UpdateAsync(ClubDTO club)
    {
        return await _http.PutAsJsonAsync("Club", club);
    }

    public async Task<HttpResponseMessage> ChangeNameAsync(ChangeClubNameDTO dto)
    {
        return await _http.PatchAsJsonAsync("Club/name", dto);
    }
}
