using MudBlazorDemo.Components;
using MudBlazor.Services;
using MudBlazorDemo;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    client.BaseAddress = new("https+http://weatherapi");
});
builder.Services.AddHttpClient<PeriodicTableApiClient>(client =>
{
    client.BaseAddress = new("https+http://periodicelementsapi");
});
builder.Services.AddHttpClient<OrganisationApiClient>(client =>
{
    client.BaseAddress = new("https+http://organisationapi");
});

builder.Services.AddMudServices();

var app = builder.Build();

app.UseOutputCache();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
