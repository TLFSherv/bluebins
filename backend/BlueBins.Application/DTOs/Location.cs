namespace Application.Dtos;

public record LocationRequest
{
    public required string Address { get; set; }
    public string? MapsId { get; set; }
    public required string Parish { get; set; }
    public string Postcode { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Details { get; set; } = string.Empty;
    public bool MakeDefault { get; set; }
}

public record LocationView
{
    public string? MapsId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Parish { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Details { get; set; } = string.Empty;
    public bool MakeDefault { get; set; }
    // This is used by the frontend to know the type of the data object
    public string Type { get; } = "location";
}

