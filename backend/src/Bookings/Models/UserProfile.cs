public class UserProfile : IEntity<string>
{
    public string Id { get; set; } = string.Empty;
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
    public Location? DefaultLocation { get; set; }
    public Schedule? DefaultSchedule { get; set; }

}