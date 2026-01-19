using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NihongoLearning.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alphabet> Alphabets { get; set; }

    public virtual DbSet<DailyQuest> DailyQuests { get; set; }

    public virtual DbSet<ShopItem> ShopItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alphabet>(entity =>
        {
            entity.HasKey(e => e.AlphabetId).HasName("PK__Alphabet__98A5117926640D90");

            entity.Property(e => e.Character).HasMaxLength(10);
            entity.Property(e => e.Level).HasMaxLength(10);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<DailyQuest>(entity =>
        {
            entity.HasKey(e => e.QuestId).HasName("PK__DailyQue__B6619A2BEA8CEAA8");

            entity.Property(e => e.QuestName).HasMaxLength(255);
            entity.Property(e => e.RequiredXp).HasColumnName("RequiredXP");
        });

        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ShopItem__727E838B60BC4174");

            entity.Property(e => e.ItemName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C9A339343");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534558B6A30").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Gems).HasDefaultValue(0);
            entity.Property(e => e.LastLearnedDate).HasColumnType("datetime");
            entity.Property(e => e.StreakCount).HasDefaultValue(0);
            entity.Property(e => e.TotalXp)
                .HasDefaultValue(0)
                .HasColumnName("TotalXP");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserKanjiProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__UserKanj__BAE29CA50878AF75");

            entity.ToTable("UserKanjiProgress");

            entity.Property(e => e.IsLearned).HasDefaultValue(false);
            entity.Property(e => e.LearnedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Alphabet).WithMany(p => p.UserKanjiProgresses)
                .HasForeignKey(d => d.AlphabetId)
                .HasConstraintName("FK__UserKanji__Alpha__3F466844");

            entity.HasOne(d => d.User).WithMany(p => p.UserKanjiProgresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserKanji__UserI__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}