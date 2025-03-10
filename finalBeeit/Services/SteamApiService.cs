using finalBeeit.Models;
using System.Net.Http;
using System.Security.Policy;
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
            string apiUrl = "https://api.steampowered.com/IStoreService/GetAppList/v1/?key=D4AB96630D5227A3D7AFA3C1D9465919&have_description_language=portuguese&max_results=50000";
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response = await httpClient.GetFromJsonAsync<SteamAppListResponse>(apiUrl, options);

                if (response?.Response?.Apps == null)
                {
                    return new List<Game>();
                }

                // Remove API-level filtering; return full list
                return response.Response.Apps
                    .Select(app => new Game
                    {
                        AppId = app.Appid,
                        Name = app.Name
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<Game>();
            }
        }
    }
}
