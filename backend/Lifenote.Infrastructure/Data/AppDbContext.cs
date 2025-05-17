using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lifenote.Core.Entities;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<activepomodoro> activepomodoros { get; set; }

    public virtual DbSet<checklistitem> checklistitems { get; set; }

    public virtual DbSet<note> notes { get; set; }

    public virtual DbSet<pomodorosession> pomodorosessions { get; set; }

    public virtual DbSet<reminder> reminders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.userid).ValueGeneratedNever();
            entity.Property(e => e.createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.email).HasMaxLength(255);
            entity.Property(e => e.phonenumber).HasMaxLength(20);
            entity.Property(e => e.username).HasMaxLength(100);
        });

        modelBuilder.Entity<activepomodoro>(entity =>
        {
            entity.HasKey(e => e.userid).HasName("activepomodoro_pkey");

            entity.ToTable("activepomodoro");

            entity.Property(e => e.userid).ValueGeneratedNever();
            entity.Property(e => e.isrunning).HasDefaultValue(true);
            entity.Property(e => e.starttime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.type).HasMaxLength(50);
            entity.Property(e => e.updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.user).WithOne(p => p.activepomodoro)
                .HasForeignKey<activepomodoro>(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activepomodoro_userid_fkey");
        });

        modelBuilder.Entity<checklistitem>(entity =>
        {
            entity.HasKey(e => e.itemid).HasName("checklistitem_pkey");

            entity.ToTable("checklistitem");

            entity.Property(e => e.itemid).ValueGeneratedNever();
            entity.Property(e => e.content).HasMaxLength(255);
            entity.Property(e => e.isdone).HasDefaultValue(false);

            entity.HasOne(d => d.note).WithMany(p => p.checklistitems)
                .HasForeignKey(d => d.noteid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("checklistitem_noteid_fkey");
        });

        modelBuilder.Entity<note>(entity =>
        {
            entity.HasKey(e => e.noteid).HasName("note_pkey");

            entity.ToTable("note");

            entity.Property(e => e.noteid).ValueGeneratedNever();
            entity.Property(e => e.colortag).HasMaxLength(50);
            entity.Property(e => e.createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.pinned).HasDefaultValue(false);
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.type).HasMaxLength(50);
            entity.Property(e => e.updatedat).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.user).WithMany(p => p.notes)
                .HasForeignKey(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("note_userid_fkey");
        });

        modelBuilder.Entity<pomodorosession>(entity =>
        {
            entity.HasKey(e => e.sessionid).HasName("pomodorosession_pkey");

            entity.ToTable("pomodorosession");

            entity.Property(e => e.sessionid).ValueGeneratedNever();
            entity.Property(e => e.endtime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.starttime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.title).HasMaxLength(255);
            entity.Property(e => e.type).HasMaxLength(50);

            entity.HasOne(d => d.user).WithMany(p => p.pomodorosessions)
                .HasForeignKey(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pomodorosession_userid_fkey");
        });

        modelBuilder.Entity<reminder>(entity =>
        {
            entity.HasKey(e => e.reminderid).HasName("reminder_pkey");

            entity.ToTable("reminder");

            entity.Property(e => e.reminderid).ValueGeneratedNever();
            entity.Property(e => e.createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.recurring).HasMaxLength(50);
            entity.Property(e => e.remindertime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.title).HasMaxLength(255);

            entity.HasOne(d => d.note).WithMany(p => p.reminders)
                .HasForeignKey(d => d.noteid)
                .HasConstraintName("reminder_noteid_fkey");

            entity.HasOne(d => d.user).WithMany(p => p.reminders)
                .HasForeignKey(d => d.userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reminder_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
