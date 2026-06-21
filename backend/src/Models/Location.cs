using System.ComponentModel.DataAnnotations;

public class Location
{
    [Required]
    public int Id { get; set; }
    public string? MapsId { get; set; }
    [Required]
    public string? AddressLine1 { get; set; }
    [Required]
    public string? Postcode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Details { get; set; }
}