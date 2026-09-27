using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    // change this to a GUID
    private readonly string _userId;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserIdService userIdService) :
        base(options)
    {
        _userId = userIdService.GetUserId();
    }
    public virtual DbSet<UserProfile> UserProfiles { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Location> Locations { get; set; }
    public virtual DbSet<Recycling> Recyclings { get; set; }
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
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever(); ;
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
            // add global query filter so users can only interact with their bookings
            entity.HasQueryFilter(x => x.UserId == _userId);

            entity.HasKey(e => e.Id).HasName("booking_pkey");

            entity.ToTable("booking", "booking");
            entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_profile_id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.Status)
            .HasDefaultValue(BookingStatus.Scheduled)
            .HasColumnName("status");
            entity.Property(e => e.DateCreated)
            // .HasDefaultValueSql("now()")
            .HasColumnName("date_created");
            entity.Property(e => e.DateModified)
            .ValueGeneratedOnAddOrUpdate()
            // .HasDefaultValueSql("now()")
            .HasColumnName("date_modified");

            entity.HasOne(d => d.UserProfile) // a booking has one user
            .WithMany(d => d.Bookings)       // a user can have many bookings
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Location) // a booking can have one location
            .WithMany()                     // a location can have many bookings
            .HasForeignKey(d => d.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Schedule) // a booking can have one schedule
            .WithMany()                     // a schedule can have many bookings
            .HasForeignKey(d => d.ScheduleId)
            .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.Recycling) // a booking has one recycling
            .WithOne(d => d.Booking)        // a recycling can have one booking
            .HasForeignKey<Recycling>(d => d.Id)
            .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("location_pkey");

            entity.ToTable("location", "booking");
            entity.Property(e => e.MapsId).HasMaxLength(256).HasColumnName("maps_id");
            entity.Property(e => e.Address).HasMaxLength(256).HasColumnName("address");
            entity.Property(e => e.Parish).HasMaxLength(32).HasColumnName("parish");
            entity.Property(e => e.Postcode).HasMaxLength(8).HasColumnName("postcode");
            entity.Property(e => e.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(9, 6);
            entity.Property(e => e.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(9, 6);
            entity.Property(e => e.Details).HasColumnName("details");

            entity.HasIndex(e => e.Postcode);
            entity.HasIndex(e => e.Address);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("schedule_pkey");

            entity.ToTable("schedule", "booking");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Frequency).HasColumnName("frequency");
        });

        modelBuilder.Entity<Recycling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recycling_item_pkey");

            entity.Property(e => e.Id).HasColumnName("booking_id");

            entity
            .HasMany(r => r.RecyclingItems) // recycling has many recycling items
            .WithOne()                          // a recycling item is only in one recycling
            .HasForeignKey(c => c.RecyclingId)
            .OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<RecyclingItem>(entity =>
        {
            entity.HasKey(e => new { e.RecyclingId, e.MaterialType });

            entity.ToTable("recycling_item", "booking");
            entity.Property(e => e.MaterialType).HasColumnName("material_type");
            entity.Property(e => e.WeightKg).HasColumnName("weight_kg");
            entity.Property(e => e.VolumeLiters).HasColumnName("volume_litres");
            entity.Property(e => e.ContaminationPercent).HasColumnName("contamination_percent");
        });
    }

}