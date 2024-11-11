using System.Text.Json.Serialization;

namespace MudBlazorDemo;

public class PeriodicTableApiClient(HttpClient httpClient)
{
    public async Task<TableElement[]> GetTableElementsAsync(CancellationToken cancellationToken = default)
    {
        List<TableElement>? elements = null;

        await foreach (var element in httpClient.GetFromJsonAsAsyncEnumerable<TableElement>("/periodicTableElement", cancellationToken))
        {
            if (element is not null)
            {
                elements ??= [];
                elements.Add(element);
            }
        }

        return elements?.ToArray() ?? [];
    }
}

public class TableElement
{
    public string Group { get; set; }
    public int Position { get; set; }
    public string Name { get; set; }
    public int Number { get; set; }

    [JsonPropertyName("small")]
    public string Sign { get; set; }
    public double Molar { get; set; }
    public IList<int> Electrons { get; set; }

    public override string ToString()
    {
        return $"{Sign} - {Name}";
    }
}
