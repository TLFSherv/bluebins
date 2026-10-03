public record RecyclingRequest : IRequest<int>
{
    public int Id { get; set; } // the same as BookingId
    public int NumberOfBags { get; set; }
    public List<RecyclingItemRequest>? RecyclingItems { get; set; }

}

public record RecyclingItemRequest
{
    public MaterialTypes MaterialType { get; set; }
    public int Quantity { get; set; }

    public RecyclingItemRequest(MaterialTypes materialType, int quantity)
    {
        MaterialType = materialType;
        Quantity = quantity;
    }
}

public record RecyclingView
{
    public int BookingId { get; set; }
    public int NumberOfBags { get; set; }
    public List<RecyclingItemView>? RecyclingItems { get; set; }

}

public record RecyclingItemView
{
    public MaterialTypes MaterialType { get; set; }
    public decimal WeightKg { get; set; }
    public decimal VolumeLiters { get; set; }
    public decimal ContaminationPercent { get; set; }
}

public static class RecyclingExtensions
{
    public static ICollection<RecyclingItem>? GetRecyclingItems(this RecyclingRequest? recycling)
    {
        if (recycling?.RecyclingItems is null) return null;
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        var recyclingItems = new List<RecyclingItem>();
        foreach (var item in recycling.RecyclingItems)
        {
            switch (item.MaterialType)
            {
                case MaterialTypes.tin:
                    weightKg = materialWeights.tin * item.Quantity;
                    break;
                case MaterialTypes.aluminium:
                    weightKg = materialWeights.aluminum * item.Quantity;
                    break;
                case MaterialTypes.glass:
                    weightKg = materialWeights.glass * item.Quantity;
                    break;
                case MaterialTypes.mixture:
                    break;
            }
            recyclingItems.Add(new RecyclingItem
            {
                MaterialType = item.MaterialType,
                WeightKg = weightKg,
                VolumeLiters = 0,
                ContaminationPercent = 0
            });
        }
        return recyclingItems;
    }
}