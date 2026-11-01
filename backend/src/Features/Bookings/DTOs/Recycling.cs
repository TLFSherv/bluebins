public record RecyclingRequest : IRequest<int>
{
    public int Id { get; set; } // the same as BookingId
    public int NumberOfBags { get; set; }
    public Dictionary<MaterialTypes, int>? MaterialQuantities { get; set; }
    public List<RecyclingItemRequest>? RecyclingItemsList =>
        MaterialQuantities?.Select(x => new RecyclingItemRequest(x.Key, x.Value)).ToList();
}



public record RecyclingView
{
    public int BookingId { get; set; }
    public int NumberOfBags { get; set; }
    public List<RecyclingItemView>? RecyclingItems { get; set; }

}



