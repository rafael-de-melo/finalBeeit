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

        //listas com filtros e paginaçao
        //atualiza a lista de jogos que contem o input de titleFilter, se titleFilter for null, exibe a lista completa
        private IEnumerable<Game> FilteredGames =>
            (games ?? new List<Game>())
            .Where(g => string.IsNullOrEmpty(titleFilter) ||
                       g.Name.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));

        // usa FilteredGames e exibe apenas 50 jogos delimitados por .Skip e .Take
        private IEnumerable<Game> PaginatedGames =>
            FilteredGames
                .Skip((currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);


        //conecta a database na inicializaçao
        protected override void OnInitialized()
        {
            context = DbFactory.CreateDbContext();
        }

        //Fetching data asynchronously (like an API call) can’t happen in the synchronous OnInitialized, so it’s done here.
        //popula games na inicializaçao para que FilteredGames e PaginatedGames tenham o que trabalhar
        protected override async Task OnInitializedAsync()
        {
            games = await SteamApiService.GetAppList();
        }

        //Tears down the database context when the component is no longer needed.
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