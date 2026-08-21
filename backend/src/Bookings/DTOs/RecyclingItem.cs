using System.ComponentModel.DataAnnotations;

public record RecyclingItemDTO
{
    public MaterialTypes MaterialType { get; set; }
    public int ItemCount { get; set; }
}
public record AddRecyclingItemRequest
{
    public int? BookingId { get; set; }
    public MaterialTypes MaterialType { get; set; }
    public decimal WeightKg { get; set; }
    public decimal VolumeLiters { get; set; }
    public decimal ContaminationPercent { get; set; }
}

public record UpdateRecyclingItemRequest : AddRecyclingItemRequest
{
    [Required]
    public int Id { get; set; }
}

public record RecyclingItemView : AddRecyclingItemRequest;