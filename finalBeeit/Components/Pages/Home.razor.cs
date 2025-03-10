﻿using finalBeeit.Data;
using finalBeeit.Models;
using finalBeeit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace finalBeeit.Components.Pages
{
    public partial class Home : ComponentBase, IAsyncDisposable
    {
        [Inject] private ISteamApiService SteamApiService { get; set; }
        [Inject] private IDbContextFactory<finalBeeitContext> DbFactory { get; set; }

        private List<Game>? games;
        private string titleFilter = string.Empty;
        private int currentPage = 1;
        private const int ItemsPerPage = 50;

        private finalBeeitContext context = default!;

        private IEnumerable<Game> FilteredGames =>
            (games ?? new List<Game>())
            .Where(g => string.IsNullOrEmpty(titleFilter) ||
                       g.Name.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));

        private IEnumerable<Game> PaginatedGames =>
            FilteredGames
                .Skip((currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);

        private int TotalPages => (int)Math.Ceiling((double)FilteredGames.Count() / ItemsPerPage);

        protected override void OnInitialized()
        {
            context = DbFactory.CreateDbContext();
        }
        protected override async Task OnInitializedAsync()
        {
            // Fetch full list once on initialization
            games = await SteamApiService.SearchGameByName(string.Empty);
        }

        public async ValueTask DisposeAsync()
        {
            if (context != null)
            {
                await context.DisposeAsync();
            }
        }

        private async Task AddGameToDb(Game game)
        {
            if (!await context.Game.AnyAsync(g => g.AppId == game.AppId))
            {
                game.LastModified = DateTime.UtcNow;
                game.PriceChangeNumber = 0;         

                context.Game.Add(game);
                await context.SaveChangesAsync();
            }
        }

        private async Task OnTitleFilterChanged()
        {
            currentPage = 1;
            await InvokeAsync(StateHasChanged);
        }

        private void NextPage()
        {
            if (currentPage < TotalPages)
            {
                currentPage++;
                StateHasChanged();
            }
        }

        private void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                StateHasChanged();
            }
        }
    }
}