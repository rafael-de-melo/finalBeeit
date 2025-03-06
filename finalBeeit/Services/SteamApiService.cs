using finalBeeit.Models;
using System.Text.Json;

namespace finalBeeit.Services
{
    public class SteamApiService : ISteamApiService
    {
        private readonly HttpClient httpClient;
        public SteamApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        async Task<List<Game>> ISteamApiService.SearchGameByName(string name)
        {
            string apiUrl = "http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format=json";
            var response = await httpClient.GetAsync(apiUrl);

            // If the response is not successful, return an empty list
            if (!response.IsSuccessStatusCode)
            {
                return new List<Game>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<SteamAppListResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var filteredGames = data?.Applist.Apps
                .Where(game => game.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(game => new Game
                {
                    AppId = game.AppId,
                    Name = game.Name
                })
                .ToList();

            return filteredGames ?? new List<Game>();
        }
    }
}
