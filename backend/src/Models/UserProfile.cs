using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class UserProfile
{
    [Required]
    public int Id { get; set; }
    public int? DefaultLocationId { get; set; }
    public int? DefaultScheduleId { get; set; }
    public bool IsDeleted { get; set; }
    public Location? DefaultLocation { get; set; }

    public Schedule? DefaultSchedule { get; set; }

}