using Application.Dtos;

public static class RecyclingExtensions
{
    public static ICollection<RecyclingItem>? GetRecyclingItems(this RecyclingRequest? recycling)
    {
        var recyclingItemsReq = recycling?.RecyclingItemsList;
        if (recyclingItemsReq is null) return null;
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        var recyclingItems = new List<RecyclingItem>();
        foreach (var item in recyclingItemsReq)
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
                Quantity = item.Quantity,
                WeightKg = weightKg,
                VolumeLiters = 0,
                ContaminationPercent = 0
            });
        }
        return recyclingItems;
    }
}