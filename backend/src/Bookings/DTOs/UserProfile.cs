using System.ComponentModel.DataAnnotations;

public record AddUserProfileRequest
{
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
}

public record UpdateUserProfileRequest : AddUserProfileRequest
{
    [Required]
    public int Id { get; set; }
}

public record UserProfileView : AddUserProfileRequest;