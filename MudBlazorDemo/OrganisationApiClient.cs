namespace MudBlazorDemo;

public class OrganisationApiClient(HttpClient httpClient)
{
    public async Task<Organisation[]> GetOrganisationsAsync(CancellationToken cancellationToken = default)
    {
        List<Organisation>? organisations = null;

        await foreach (var organisation in httpClient.GetFromJsonAsAsyncEnumerable<Organisation>("/organisations", cancellationToken))
        {
            if (organisation is not null)
            {
                organisations ??= [];
                organisations.Add(organisation);
            }
        }

        return organisations?.ToArray() ?? [];
    }

    public async Task<Organisation[]> GetOrganisationsWithCachingAsync(CancellationToken cancellationToken = default)
    {
        List<Organisation>? organisations = null;

        await foreach (var organisation in httpClient.GetFromJsonAsAsyncEnumerable<Organisation>("/orgswithcaching", cancellationToken))
        {
            if (organisation is not null)
            {
                organisations ??= [];
                organisations.Add(organisation);
            }
        }

        return organisations?.ToArray() ?? [];
    }
}

public record Organisation(Guid OrganisationGuid, string OrganisationName, int NumberOfConnectors, bool Healthy);