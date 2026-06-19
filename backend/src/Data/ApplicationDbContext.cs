using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }
    public virtual DbSet<UserSetting> UserSettings { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<RecyclingItem> RecyclingItems { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("recycler");

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_id_pkey");
        });
    }

}