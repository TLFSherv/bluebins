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

public record RecyclingItemView
{
    public MaterialTypes MaterialType { get; set; }
    public int Quantity { get; set; }
}