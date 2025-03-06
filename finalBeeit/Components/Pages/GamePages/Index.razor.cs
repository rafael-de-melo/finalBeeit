using finalBeeit.Data;
using finalBeeit.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace finalBeeit.Components.Pages.GamePages
{
    public partial class Index
    {
        private PaginationState pagination = new PaginationState { ItemsPerPage = 2 };

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
