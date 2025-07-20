using System;
using System.Collections.Generic;
using Lifenote.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Lifenote.Data.Data;

public partial class LifenoteDbContext : DbContext
{
    public LifenoteDbContext()
    {
    }

    public LifenoteDbContext(DbContextOptions<LifenoteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Habit> Habits { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Timer> Timers { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=lifenote;Username=postgres;Password=post123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Habit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Habits_pkey");

            entity.HasIndex(e => e.IsActive, "IX_Habits_IsActive");

            entity.HasIndex(e => e.UserId, "IX_Habits_UserId");

            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .HasDefaultValueSql("'#3498db'::character varying");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CurrentStreak).HasDefaultValue(0);
            entity.Property(e => e.Frequency).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LongestStreak).HasDefaultValue(0);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TargetCount).HasDefaultValue(1);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Habits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Habits_UserId_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Note_pkey");

            entity.ToTable("Note");

            entity.HasIndex(e => e.Category, "IX_Note_Category");

            entity.HasIndex(e => e.CreatedAt, "IX_Note_CreatedAt");

            entity.HasIndex(e => e.UserId, "IX_Note_UserId");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsArchived).HasDefaultValue(false);
            entity.Property(e => e.IsPinned).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Note_UserId_fkey");
        });

        modelBuilder.Entity<Timer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Timer_pkey");

            entity.ToTable("Timer");

            entity.HasIndex(e => e.SessionType, "IX_Timer_SessionType");

            entity.HasIndex(e => e.StartTime, "IX_Timer_StartTime");

            entity.HasIndex(e => e.UserId, "IX_Timer_UserId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.SessionType).HasMaxLength(20);

            entity.HasOne(d => d.User).WithMany(p => p.Timers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Timer_UserId_fkey");
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserInfo_pkey");

            entity.ToTable("UserInfo");

            entity.HasIndex(e => e.Email, "UserInfo_Email_key").IsUnique();

            entity.HasIndex(e => e.Username, "UserInfo_Username_key").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
