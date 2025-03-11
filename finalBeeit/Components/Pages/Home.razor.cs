using finalBeeit.Data;
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
        private finalBeeitContext context = default!;

        // variaveis paginaçao
        private string titleFilter = string.Empty;
        private int currentPage = 1;
        private const int ItemsPerPage = 50;
        private int TotalPages => (int)Math.Ceiling((double)FilteredGames.Count() / ItemsPerPage);

        // variaveis notificação sucesso/falha
        private string notificationMessage = string.Empty;
        private bool isSuccess = false;

        private IEnumerable<Game> FilteredGames =>
            (games ?? new List<Game>())
            .Where(g => string.IsNullOrEmpty(titleFilter) ||
                       g.Name.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));
        private IEnumerable<Game> PaginatedGames =>
            FilteredGames
                .Skip((currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);


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
            try
            {
                if (!await context.Game.AnyAsync(g => g.AppId == game.AppId))
                {
                    game.LastModified = DateTime.UtcNow;
                    game.PriceChangeNumber = 0;

                    context.Game.Add(game);
                    await context.SaveChangesAsync();

                    notificationMessage = $"Jogo '{game.Name}' adicionado com sucesso";
                    isSuccess = true;
                }
                else
                {
                    notificationMessage = $"O jogo '{game.Name}' já está no banco de dados";
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                notificationMessage = $"Erro ao adicionar '{game.Name}': {ex.Message}";
                isSuccess = false;
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