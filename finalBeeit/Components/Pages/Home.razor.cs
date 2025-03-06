using finalBeeit.Data;
using finalBeeit.Models;
using finalBeeit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace finalBeeit.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] private ISteamApiService SteamApiService { get; set; }

        private string searchTerm = "";
        private List<Game>? games;

        protected override async Task OnInitializedAsync()
        {
            await SearchGames();
        }

        private async Task SearchGames()
        {
            games = await SteamApiService.SearchGameByName(searchTerm);
        }
    }
}
