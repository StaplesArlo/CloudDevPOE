#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ST10404431CLDV6211POE.Models;

public partial class EventEaseDBContext : DbContext
{
    public EventEaseDBContext(DbContextOptions<EventEaseDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventID).HasName("PK__Events__7944C8707C81128E");

            entity.ToTable(tb => tb.HasTrigger("GenerateEventID"));

            entity.Property(e => e.EventID)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EventDate).HasColumnType("date");
            entity.Property(e => e.EventName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VenueID)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Venue).WithMany(p => p.Events)
                .HasForeignKey(d => d.VenueID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__VenueID__4BAC3F29");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.VenueID).HasName("PK__Venues__3C57E5D24CFF3C81");

            entity.Property(e => e.VenueID)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageURL)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VenueName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}