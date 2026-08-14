public class BookingHelpers : IBookingHelpers
{
    public (decimal WeightKg, decimal VolumeLiters) CalculateWeightAndVolume(MaterialTypes material, int itemCount)
    {
        var materialWeights = new { tin = 0, aluminum = 0, glass = 0 };
        decimal weightKg = 0;
        switch (material)
        {
            case MaterialTypes.tin:
                weightKg = materialWeights.tin * itemCount;
                break;
            case MaterialTypes.aluminum:
                weightKg = materialWeights.aluminum * itemCount;
                break;
            case MaterialTypes.glass:
                weightKg = materialWeights.glass * itemCount;
                break;
            case MaterialTypes.mixture:
                break;
        }
        return (weightKg, 0);
    }
}