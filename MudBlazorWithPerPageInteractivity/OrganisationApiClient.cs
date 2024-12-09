namespace MudBlazorWithPerPageInteractivity;

public class OrganisationApiClient(HttpClient httpClient)
{
    public async Task<Organisation[]> GetOrganisationsAsync(CancellationToken cancellationToken = default)
    {
        List<Organisation>? organisations = null;

        await foreach (var organisation in httpClient.GetFromJsonAsAsyncEnumerable<Organisation>("/organisations",
                           cancellationToken))
        {
            if (organisation is not null)
            {
                organisations ??= [];
                organisations.Add(organisation);
            }
        }

        return organisations?.ToArray() ?? [];
    }

    public async Task<Connector[]> GetConnectorsAsync(Guid organisationGuid,
        CancellationToken cancellationToken = default)
    {
        List<Connector>? connectors = null;

        await foreach (var connector in httpClient.GetFromJsonAsAsyncEnumerable<Connector>(
                           "/connectorswithcaching?organisationGuid=" + organisationGuid,
                           cancellationToken))
        {
            if (connector is not null)
            {
                connectors ??= [];
                connectors.Add(connector);
            }
        }

        return connectors?.ToArray() ?? [];
    }
}

public record Organisation(Guid OrganisationGuid, string OrganisationName, int NumberOfConnectors, bool Healthy);

public record Connector(Guid OrganisationGuid, Guid ConnectorGuid, string Name, bool Healthy);