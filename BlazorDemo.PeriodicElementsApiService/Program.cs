using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapGet("/periodicTableElement", () =>
{
    return new List<TableElement>
    {
       new()
       {
           Number = 1,
           Sign = "H",
           Name = "Hydrogen",
           Position = 0,
           Molar = 1.00794,
           Group = "Other",
       },
        new()
        {
            Number = 2,
            Sign = "He",
            Name = "Helium",
            Position = 17,
            Molar = 4.002602,
            Group = "Noble Gas (p)"
        },
        new()
        {
            Number = 3,
            Sign = "Li",
            Name = "Lithium",
            Position = 0,
            Molar = 6.941,
            Group = "Alkaline Earth Metal (s)"
        },
        new()
        {
            Number = 4,
            Sign = "Be",
            Name = "Beryllium",
            Position = 1,
            Molar = 9.012182,
            Group = "Alkaline Metal (s)"
        },
        new()
        {
            Number = 5,
            Sign = "B",
            Name = "Boron",
            Position = 12,
            Molar = 10.811,
            Group = "Metalloid Boron (p)"
        },
        new()
        {
            Number = 6,
            Sign = "C",
            Name = "Carbon",
            Position = 13,
            Molar = 12.0107,
            Group = "Nonmetal Carbon (p)"
        },
        new()
        {
            Number = 7,
            Sign = "N",
            Name = "Nitrogen",
            Position = 14,
            Molar = 14.0067,
            Group = "Nonmetal Pnictogen (p)"
        },
        new()
        {
            Number = 8,
            Sign = "O",
            Name = "Oxygen",
            Position = 15,
            Molar = 15.9994,
            Group = "Nonmetal Chalcogen (p)"
        },
        new()
        {
            Number = 9,
            Sign = "F",
            Name = "Fluorine",
            Position = 16,
            Molar = 18.998404,
            Group = "Halogen (p)"
        },
        new()
        {
            Number = 10,
            Sign = "Ne",
            Name = "Neon",
            Position = 17,
            Molar = 20.1797,
            Group = "Noble Gas (p)"
        }
    }.ToArray();
});

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.Run();

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