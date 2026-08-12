using System.ComponentModel.DataAnnotations;

public record LocationDTO
{
    public int? LocationId { get; set; }
    public string? MapsId { get; set; }
    public string Address { get; set; }
    public string Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
    public bool SetAsDefault { get; set; }
}
public record AddLocationRequest
{
    public string? MapsId { get; set; }
    public string? Address { get; set; }
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
}

public record UpdateLocationRequest : AddLocationRequest
{
    public int LocationId { get; set; }
}

public record LocationView : AddLocationRequest;

