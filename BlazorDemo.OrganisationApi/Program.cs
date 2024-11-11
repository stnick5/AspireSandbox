using Bogus;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

//builder.AddRedisDistributedCache("cache");

// Add services to the container.

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.MapGet("/organisations", async (IDistributedCache cache) =>
//{
//    var cachedOrganisations = await cache.GetAsync("organisations");

//    if (cachedOrganisations is null)
//    {
//        var organisationFactory = new Faker<Organisation>().CustomInstantiator(f => new Organisation(f.Random.Uuid(), f.Company.CompanyName()));
//        var organisations = organisationFactory.Generate(100);

//        await cache.SetAsync("organisations", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(organisations)), new()
//        {
//            AbsoluteExpiration = DateTime.Now.AddSeconds(10)
//        });

//        return organisations;
//    }

//    return JsonSerializer.Deserialize<IEnumerable<Organisation>>(cachedOrganisations);
//})
//    .WithName("GetOrganisations");

app.MapGet("/organisations", () =>
{
    var organisationFactory = new Faker<Organisation>().CustomInstantiator(f => new Organisation(f.Random.Uuid(), f.Company.CompanyName()));
    var organisations = organisationFactory.Generate(100);

    return organisations;
});


app.Run();

record Organisation(Guid OrganisationGuid, string OrganisationName);