using Smab.TTInfo.CunninghamCup.Shared.Services;
using Smab.TTInfo.CunninghamCup.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

//builder.Services.AddLocalization();
//builder.Services.AddHealthChecks();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

builder.Services.AddTournamentService();
builder.Services.AddTTClubsService();
builder.Services.AddScoped<AppState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	_ = app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	_ = app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.Services
	.GetRequiredService<ITournamentService>()
	.SeedRandomTournament(22).DrawGroups(4);

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.CunninghamCup.Shared._Imports).Assembly);

app.Run();
