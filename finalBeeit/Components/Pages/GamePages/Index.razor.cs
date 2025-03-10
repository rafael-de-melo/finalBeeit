using finalBeeit.Data;
using finalBeeit.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.EntityFrameworkCore;

namespace finalBeeit.Components.Pages.GamePages
{
    public partial class Index : IAsyncDisposable
    {
        [Inject] IDbContextFactory<finalBeeit.Data.finalBeeitContext> DbFactory { get; set; }



        private PaginationState pagination = new PaginationState { ItemsPerPage = 10 };
        
        private string titleFilter = string.Empty;
        private IQueryable<Game> FilteredGames =>
            context.Game.Where(m => m.Name!.Contains(titleFilter));
        
        private finalBeeitContext context = default!;
        
        protected override void OnInitialized()
        {
            context = DbFactory.CreateDbContext();
        }
        public async ValueTask DisposeAsync() => await context.DisposeAsync();
    }
}
