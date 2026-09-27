public class RecyclingItem
{
    public int RecyclingId { get; set; }
    public MaterialTypes MaterialType { get; set; }
    public decimal WeightKg { get; set; }
    public decimal VolumeLiters { get; set; }
    public decimal ContaminationPercent { get; set; } // add default
}

public enum MaterialTypes
{
    tin = 1,
    aluminium = 2,
    glass = 3,
    mixture = 4
}