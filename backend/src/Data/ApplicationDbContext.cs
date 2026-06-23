using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }
    public virtual DbSet<UserProfile> UserProfiles { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<RecyclingItem> RecyclingItems { get; set; }
    public virtual DbSet<Schedule> Schedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Maps all Identity tables and keys first
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_profile_pkey");

            entity.ToTable("user_profile", "booking");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DefaultLocationId).HasColumnName("default_location_id");
            entity.Property(e => e.DefaultScheduleId).HasColumnName("default_schedule_id");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            entity.HasOne(d => d.DefaultLocation)
            .WithMany() // Leaves it unidirectional if Locations doesn't need a List<User>
            .HasForeignKey(d => d.DefaultLocationId)
            .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.DefaultSchedule)
            .WithMany()
            .HasForeignKey(d => d.DefaultScheduleId)
            .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("booking_pkey");

            entity.ToTable("booking", "booking");
            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
            entity.Property(e => e.UserProfileId).HasColumnName("user_profile_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.Status)
            .HasDefaultValue(BookingStatus.Scheduled)
            .HasColumnName("status");
            entity.Property(e => e.CollectionDate).HasColumnName("collection_date");
            entity.Property(e => e.DateCreated)
            .HasDefaultValueSql("now()")
            .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("now()")
            .HasColumnName("date_modified");

            entity.HasOne(d => d.UserProfile) // a booking has one user
            .WithMany()                 // a user can have many bookings
            .HasForeignKey(d => d.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Location) // a booking can have one location
            .WithMany()                     // a location can have many bookings
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Schedule) // a booking can have one schedule
            .WithMany()                     // a schedule can have many bookings
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location", "booking");
            entity.Property(e => e.MapsId).HasColumnName("maps_id");
            entity.Property(e => e.AddressLine1).HasColumnName("address_line_1");
            entity.Property(e => e.Postcode).HasColumnName("postcode");
            entity.Property(e => e.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(9, 6);
            entity.Property(e => e.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(9, 6);
            entity.Property(e => e.Details).HasColumnName("details");

            entity.HasIndex(e => e.Postcode);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("schedule_pkey");

            entity.ToTable("schedule", "booking");
            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Frequency).HasColumnName("frequency");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });

        modelBuilder.Entity<RecyclingItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recycling_item_pkey");

            entity.ToTable("recycling_item", "booking");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.MaterialType).HasColumnName("material_type");
            entity.Property(e => e.WeightKg).HasColumnName("weight_kg");
            entity.Property(e => e.VolumeLiters).HasColumnName("volume_litres");
            entity.Property(e => e.ContaminationPercent).HasColumnName("contamination_percent");

            entity
            .HasOne(e => e.Booking) // a recycling item has one booking
            .WithMany()             // a booking has many recycling items
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }

}