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

public static class RecyclingItemExtensions
{
    public static List<AddRecyclingItemRequest>? ToRequest(this List<RecyclingItemDTO>? recyclingItemDTO)
    {
        if (recyclingItemDTO is null) return null;
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        var result = new List<AddRecyclingItemRequest>();
        foreach (var item in recyclingItemDTO)
        {
            switch (item.MaterialType)
            {
                case MaterialTypes.tin:
                    weightKg = materialWeights.tin * item.ItemCount;
                    break;
                case MaterialTypes.aluminium:
                    weightKg = materialWeights.aluminum * item.ItemCount;
                    break;
                case MaterialTypes.glass:
                    weightKg = materialWeights.glass * item.ItemCount;
                    break;
                case MaterialTypes.mixture:
                    break;
            }
            result.Add(new AddRecyclingItemRequest
            {
                MaterialType = item.MaterialType,
                WeightKg = weightKg,
                VolumeLiters = 0,
                ContaminationPercent = 0
            });

        }
        return result;
    }
}