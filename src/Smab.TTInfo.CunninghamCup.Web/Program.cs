using Smab.TTInfo.CunninghamCup.Shared.Services;
using Smab.TTInfo.CunninghamCup.Web.Components;
using Microsoft.Extensions.Options;
using Smab.TTInfo.CunninghamCup.Web.Services;

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
builder.Services.Configure<TTInfoOptions>(builder.Configuration.GetSection("TTInfoOptions"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TTInfoOptions>>().Value);

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

_ = app.Services
	.GetRequiredService<ITournamentService>()
	.LoadTournamentFromJsonAsync();
	//.SeedRandomTournament(22).DrawGroups(4);

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode()
	.AddAdditionalAssemblies(typeof(Smab.TTInfo.CunninghamCup.Shared._Imports).Assembly);

app.MapTournamentApiEndpoints();

app.Run();
