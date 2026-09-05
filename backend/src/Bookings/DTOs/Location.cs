public record LocationRequest : IRequest<int>
{
    public int Id { get; set; }
    public string? MapsId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
}

public record LocationView
{
    public string? MapsId { get; set; }
    public string? AddressLine1 { get; set; }
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
}

