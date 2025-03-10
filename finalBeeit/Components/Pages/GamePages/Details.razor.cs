using finalBeeit.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace finalBeeit.Components.Pages.GamePages
{
    public partial class Details
    {
        [Inject] IDbContextFactory<finalBeeit.Data.finalBeeitContext> DbFactory { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        private Game? game;

        [SupplyParameterFromQuery]
        private int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            using var context = DbFactory.CreateDbContext();
            game = await context.Game.FirstOrDefaultAsync(m => m.Id == Id);

            if (game is null)
            {
                NavigationManager.NavigateTo("notfound");
            }
        }
    }
}
