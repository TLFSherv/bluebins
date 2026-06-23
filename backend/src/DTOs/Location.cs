
using System.ComponentModel.DataAnnotations;

public record AddLocationRequest
{
    public string? MapsId { get; set; }
    [Required]
    public string? Address { get; set; }
    [Required]
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
}

public record UpdateLocationRequest : AddLocationRequest
{
    [Required]
    public int LocationId { get; set; }
}

public record LocationView : AddLocationRequest;

