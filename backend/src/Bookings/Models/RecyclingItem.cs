using System.ComponentModel.DataAnnotations;

public class RecyclingItem
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int BookingId { get; set; }
    public MaterialTypes MaterialType { get; set; }
    public decimal WeightKg { get; set; }
    public decimal VolumeLiters { get; set; }
    public decimal ContaminationPercent { get; set; } // add default

    public Booking? Booking { get; set; }
}
public enum MaterialTypes
{
    tin = 1,
    aluminium = 2,
    glass = 3,
    mixture = 4
}