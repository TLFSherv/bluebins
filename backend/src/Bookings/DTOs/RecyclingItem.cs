public record RecyclingItemDTO
{
    public MaterialTypes MaterialType { get; set; }
    public int MaterialCount { get; set; }
}
public record AddRecyclingItemRequest
{
    public int BookingId { get; set; }
    public MaterialTypes MaterialType { get; set; }
    public int MaterialCount { get; set; }
    public decimal WeightKg { get; set; }
    public decimal VolumeLiters { get; set; }
    public decimal ContaminationPercent { get; set; }
}

public record UpdateRecyclingItemRequest : AddRecyclingItemRequest;
public record RecyclingItemView : AddRecyclingItemRequest;

public static class RecyclingItemExtensions
{
    public static void CalculateWeightAndVolume(this ICollection<AddRecyclingItemRequest>? recyclingItemDTO)
    {
        if (recyclingItemDTO is null) return;
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        var result = new List<AddRecyclingItemRequest>();
        foreach (var item in recyclingItemDTO)
        {
            switch (item.MaterialType)
            {
                case MaterialTypes.tin:
                    weightKg = materialWeights.tin * item.MaterialCount;
                    break;
                case MaterialTypes.aluminium:
                    weightKg = materialWeights.aluminum * item.MaterialCount;
                    break;
                case MaterialTypes.glass:
                    weightKg = materialWeights.glass * item.MaterialCount;
                    break;
                case MaterialTypes.mixture:
                    break;
            }
            result.Add(new AddRecyclingItemRequest
            {
                MaterialType = item.MaterialType,
                MaterialCount = item.MaterialCount,
                WeightKg = weightKg,
                VolumeLiters = 0,
                ContaminationPercent = 0
            });
        }
    }
}