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

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonContent> LessonContents { get; set; }

    public virtual DbSet<ShopItem> ShopItems { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserKanjiProgress> UserKanjiProgresses { get; set; }

    public virtual DbSet<UserLessonProgress> UserLessonProgresses { get; set; }

    // ========== THÊM MỚI ==========
    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionOption> QuestionOptions { get; set; }

    public virtual DbSet<LearningSession> LearningSessions { get; set; }
    // ==============================

    public virtual DbSet<Achievement> Achievements { get; set; }
    public virtual DbSet<UserAchievement> UserAchievements { get; set; }
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

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACD02E0ED37E");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GemsReward).HasDefaultValue(5);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LessonName).HasMaxLength(100);
            entity.Property(e => e.LevelRequired).HasMaxLength(10);
            entity.Property(e => e.XpReward).HasDefaultValue(10);

            entity.HasOne(d => d.Topic).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lessons_Topics");
        });

        modelBuilder.Entity<LessonContent>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__LessonCo__2907A81EC41D3F74");

            entity.HasOne(d => d.Alphabet).WithMany(p => p.LessonContents)
                .HasForeignKey(d => d.AlphabetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonContents_Alphabets");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonContents)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LessonContents_Lessons");
        });

        modelBuilder.Entity<ShopItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ShopItem__727E838B60BC4174");

            entity.Property(e => e.ItemName).HasMaxLength(100);
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__Topics__022E0F5D4BEF7ED9");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IconUrl).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TopicName).HasMaxLength(100);
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

        modelBuilder.Entity<UserLessonProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__UserLess__BAE29CA56A666D30");

            entity.ToTable("UserLessonProgress");

            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);

            entity.HasOne(d => d.Lesson).WithMany(p => p.UserLessonProgresses)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLessonProgress_Lessons");

            entity.HasOne(d => d.User).WithMany(p => p.UserLessonProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLessonProgress_Users");
        });


        // ========== THÊM MỚI VÀO CUỐI METHOD ==========
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8C");

            entity.Property(e => e.QuestionType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.QuestionText).HasMaxLength(500).IsRequired();
            entity.Property(e => e.AudioUrl).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Points).HasDefaultValue(10);

            entity.HasOne(q => q.Lesson)
                .WithMany()
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuestionOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Question__92C7A1DF");

            entity.Property(e => e.OptionText).HasMaxLength(200).IsRequired();

            entity.HasOne(o => o.Question)
                .WithMany(q => q.QuestionOptions)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LearningSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Learning__C9F49290");

            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Lives).HasDefaultValue(5);
            entity.Property(e => e.StartTime).HasDefaultValueSql("GETDATE()");

            entity.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Lesson)
                .WithMany()
                .HasForeignKey(s => s.LessonId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ⬇️ THÊM MỚI
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId);

            entity.Property(e => e.AchievementName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Condition).HasMaxLength(100).IsRequired();
            entity.Property(e => e.IconUrl).HasMaxLength(500);
            entity.Property(e => e.Rarity).HasMaxLength(20).HasDefaultValue("Common");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.UserAchievementId);

            entity.Property(e => e.CurrentProgress).HasDefaultValue(0);
            entity.Property(e => e.IsUnlocked).HasDefaultValue(false);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.User)
                .WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Achievement)
                .WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index để tránh duplicate
            entity.HasIndex(e => new { e.UserId, e.AchievementId }).IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
