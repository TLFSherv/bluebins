using System.ComponentModel.DataAnnotations;

public class UserSetting
{
    [Required]
    public int UserId { get; set; }
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
}