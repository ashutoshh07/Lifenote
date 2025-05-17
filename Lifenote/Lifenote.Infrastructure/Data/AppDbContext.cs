using System;
using System.Collections.Generic;
using Lifenote.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lifenote.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activepomodoro> Activepomodoros { get; set; }

    public virtual DbSet<Checklistitem> Checklistitems { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Pomodorosession> Pomodorosessions { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=lifenote;Username=postgres;Password=post123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activepomodoro>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("activepomodoro_pkey");

            entity.ToTable("activepomodoro");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Isrunning)
                .HasDefaultValue(true)
                .HasColumnName("isrunning");
            entity.Property(e => e.Starttime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.User).WithOne(p => p.Activepomodoro)
                .HasForeignKey<Activepomodoro>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("activepomodoro_userid_fkey");
        });

        modelBuilder.Entity<Checklistitem>(entity =>
        {
            entity.HasKey(e => e.Itemid).HasName("checklistitem_pkey");

            entity.ToTable("checklistitem");

            entity.Property(e => e.Itemid)
                .ValueGeneratedNever()
                .HasColumnName("itemid");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.Isdone)
                .HasDefaultValue(false)
                .HasColumnName("isdone");
            entity.Property(e => e.Noteid).HasColumnName("noteid");

            entity.HasOne(d => d.Note).WithMany(p => p.Checklistitems)
                .HasForeignKey(d => d.Noteid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("checklistitem_noteid_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Noteid).HasName("note_pkey");

            entity.ToTable("note");

            entity.Property(e => e.Noteid)
                .ValueGeneratedNever()
                .HasColumnName("noteid");
            entity.Property(e => e.Colortag)
                .HasMaxLength(50)
                .HasColumnName("colortag");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Pinned)
                .HasDefaultValue(false)
                .HasColumnName("pinned");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Notes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("note_userid_fkey");
        });

        modelBuilder.Entity<Pomodorosession>(entity =>
        {
            entity.HasKey(e => e.Sessionid).HasName("pomodorosession_pkey");

            entity.ToTable("pomodorosession");

            entity.Property(e => e.Sessionid)
                .ValueGeneratedNever()
                .HasColumnName("sessionid");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Endtime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("endtime");
            entity.Property(e => e.Starttime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("starttime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Pomodorosessions)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pomodorosession_userid_fkey");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Reminderid).HasName("reminder_pkey");

            entity.ToTable("reminder");

            entity.Property(e => e.Reminderid)
                .ValueGeneratedNever()
                .HasColumnName("reminderid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Noteid).HasColumnName("noteid");
            entity.Property(e => e.Recurring)
                .HasMaxLength(50)
                .HasColumnName("recurring");
            entity.Property(e => e.Remindertime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("remindertime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Note).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.Noteid)
                .HasConstraintName("reminder_noteid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reminder_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Userid)
                .ValueGeneratedNever()
                .HasColumnName("userid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
