using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieBookApi.Models.Db;

public partial class MovieBookDbContext : DbContext
{
    public MovieBookDbContext()
    {
    }

    public MovieBookDbContext(DbContextOptions<MovieBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Theatre> Theatres { get; set; }

    public virtual DbSet<TheatreMovie> TheatreMovies { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;database=MovieBookDB;Integrated Security=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951ACD434A8402");

            entity.Property(e => e.BookingId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("BookingID");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MovieID");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Seats).IsUnicode(false);
            entity.Property(e => e.ShowTime)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TheatreId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("TheatreID");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Movie).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__MovieI__10566F31");

            entity.HasOne(d => d.Theatre).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TheatreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__Theatr__0697FACD");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bookings__UserID__0F624AF8");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupons__384AF1DA7BA4B831");

            entity.Property(e => e.CouponId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("CouponID");
            entity.Property(e => e.CouponCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__4BD2943A58309B10");

            entity.Property(e => e.MovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("MovieID");
            entity.Property(e => e.Casting)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Language)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Trailer)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A58A86982D6");

            entity.HasIndex(e => e.TransactionId, "UQ__Payments__55433A4AC60806B6").IsUnique();

            entity.Property(e => e.PaymentId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.BookingId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("BookingID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TransactionID");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payments)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Bookin__17F790F9");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AED9B1B4EB");

            entity.Property(e => e.ReviewId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("ReviewID");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MovieID");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Movie).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__MovieID__1EA48E88");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__UserID__1DB06A4F");
        });

        modelBuilder.Entity<Theatre>(entity =>
        {
            entity.HasKey(e => e.TheatreId).HasName("PK__Theatres__13B38381C16CEAD1");

            entity.Property(e => e.TheatreId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("TheatreID");
            entity.Property(e => e.Area)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Screens).HasDefaultValue(1);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TheatreMovie>(entity =>
        {
            entity.HasKey(e => e.TheatreMovieId).HasName("PK__TheatreM__D4B5FCA9B08AC6C5");

            entity.Property(e => e.TheatreMovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("TheatreMovieID");
            entity.Property(e => e.AvailableSeats)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MovieId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("MovieID");
            entity.Property(e => e.ShowTimes).IsUnicode(false);
            entity.Property(e => e.TheatreId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("TheatreID");

            entity.HasOne(d => d.Movie).WithMany(p => p.TheatreMovies)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TheatreMo__Movie__2A164134");

            entity.HasOne(d => d.Theatre).WithMany(p => p.TheatreMovies)
                .HasForeignKey(d => d.TheatreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TheatreMo__Theat__29221CFB");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC30A5A071");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534CA47A072").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(CONVERT([varchar](36),newid()))")
                .HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("User");
            entity.Property(e => e.SecurityAnswer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SecurityQuestion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
