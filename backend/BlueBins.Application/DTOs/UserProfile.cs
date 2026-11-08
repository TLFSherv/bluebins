namespace Application.Dtos;

public record UserProfileRequest
{
    public int Id { get; set; }
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
}

public record UserProfileView
{
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
}