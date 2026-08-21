public interface IBookingHelpers
{
    public (decimal WeightKg, decimal VolumeLiters) CalculateWeightAndVolume(MaterialTypes material, int itemCount);
}