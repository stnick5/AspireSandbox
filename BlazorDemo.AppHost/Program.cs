var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var weatherApi = builder.AddProject<Projects.BlazorDemo_WeatherApiService>("weatherapi");
var periodicElementsApi = builder.AddProject<Projects.BlazorDemo_PeriodicElementsApiService>("periodicelementsapi");
var organisationApi = builder.AddProject<Projects.BlazorDemo_OrganisationApi>("organisationapi")
    .WithReference(cache);

builder.AddProject<Projects.BlazorDemoRendering>("BlazorRendering");

builder.AddProject<Projects.BlazorDemoWithRedis>("blazordemowithredis")
    .WithExternalHttpEndpoints()
    .WithReference(weatherApi)
    .WithReference(cache);

builder.AddProject<Projects.MudBlazorDemo>("mudblazordemo")
    .WithExternalHttpEndpoints()
    .WithReference(weatherApi)
    .WithReference(periodicElementsApi)
    .WithReference(organisationApi)
    .WithReference(cache);

builder.AddProject<Projects.MudBlazorWithPerPageInteractivity>("mudblazorwithperpageinteractivity")
    .WithExternalHttpEndpoints()
    .WithReference(weatherApi)
    .WithReference(organisationApi)
    .WithReference(cache);


builder.Build().Run();
