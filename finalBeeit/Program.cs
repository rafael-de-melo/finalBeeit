using finalBeeit.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using finalBeeit.Data;
using finalBeeit.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<finalBeeitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("finalBeeitContext") ?? throw new InvalidOperationException("Connection string 'finalBeeitContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddHttpClient<ISteamApiService, SteamApiService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
