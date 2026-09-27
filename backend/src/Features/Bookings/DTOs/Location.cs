public record LocationRequest : IRequest<int>
{
    public int Id { get; set; }
    public required string Address { get; set; }
    public string? MapsId { get; set; }
    public required string Parish { get; set; }
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
    public bool MakeDefault { get; set; }
}

public record LocationView
{
    public string? MapsId { get; set; }
    public required string Address { get; set; }
    public required string Parish { get; set; }
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
    public bool IsDefault { get; set; }
}

