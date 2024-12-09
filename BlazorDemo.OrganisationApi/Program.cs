using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisDistributedCache("cache");

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/organisations", () =>
{
    var organisationFactory = new Faker<Organisation>().CustomInstantiator(f => new Organisation(f.Random.Uuid(), f.Company.CompanyName(), f.Random.Int(1, 4), f.Random.Bool()));
    var organisations = organisationFactory.Generate(100);

    return organisations;
});

app.MapGet("/connectorswithcaching",
    async ([FromServices] IDistributedCache cache, [FromServices] IConnectionMultiplexer redis, Guid organisationGuid) =>
    {
        var connectorNames = new List<string>
        {
            "Net2", "Sage", "BioStar", "SignInApp", "PeopleHR", "Avigilon"
        };
        
        var lockKey = "connectorbyorganisationLock";
        var db = redis.GetDatabase();
        var lockAcquired = await db.LockTakeAsync(lockKey, Environment.MachineName, TimeSpan.FromSeconds(10));

        if (!lockAcquired)
        {
            return Results.StatusCode(423);
        }

        try
        {
            var cacheKey = "connectorbyorganisation";
            var cachedConnectors = await cache.GetStringAsync(cacheKey);

            if (cachedConnectors is not null)
            {
                var deserializedResponse = JsonSerializer.Deserialize<List<Connector>>(cachedConnectors);
                return Results.Ok(deserializedResponse);
            }

            var connectorFactory = new Faker<Connector>()
                .CustomInstantiator(f =>
                    new Connector(organisationGuid, f.Random.Uuid(), f.PickRandom(connectorNames), f.Random.Bool()));

            var connectors = connectorFactory.GenerateBetween(1, 4);

            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(connectors), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });
            
            return Results.Ok(connectors);
        }
        finally
        {
            await db.LockReleaseAsync(lockKey, Environment.MachineName);
        }
    });

app.MapGet("/orgswithcaching", async ([FromServices] IDistributedCache cache, [FromServices] IConnectionMultiplexer redis) =>
{
    var lockKey = "organisationsLock";
    var db = redis.GetDatabase();
    var lockAcquired = await db.LockTakeAsync(lockKey, Environment.MachineName, TimeSpan.FromSeconds(10));

    if (!lockAcquired)
    {
        return Results.StatusCode(423);
    }

    try
    {
        var cacheKey = "organisations";
        var cachedOrganisations = await cache.GetStringAsync(cacheKey);

        if (cachedOrganisations is not null)
        {
            var deserializedResponse = JsonSerializer.Deserialize<List<Organisation>>(cachedOrganisations);
            return Results.Ok(deserializedResponse);
        }

        var organisationFactory = new Faker<Organisation>().CustomInstantiator(f =>
            new Organisation(f.Random.Uuid(), f.Company.CompanyName(), f.Random.Int(1, 4), f.Random.Bool()));
        
        var organisations = organisationFactory.Generate(100);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(organisations), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
        });

        return Results.Ok(organisations);
    }
    finally
    {
        await db.LockReleaseAsync(lockKey, Environment.MachineName);
    }
});


app.Run();

record Organisation(Guid OrganisationGuid, string OrganisationName, int NumberOfConnectors, bool Healthy);

record Connector(Guid OrganisationGuid, Guid ConnectorGuid, string Name, bool Healthy);