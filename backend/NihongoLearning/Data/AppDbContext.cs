using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NihongoLearning.Models;

namespace NihongoLearning.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Alphabet> Alphabets { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<UserItem> UserItems { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Node> Nodes { get; set; }
    public DbSet<UserLessonProgress> UserLessonProgresses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Quan hệ 1-1 giữa User và UserProfile

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
    .HasOne(u => u.UserProfile)
    .WithOne(p => p.User)
    .HasForeignKey<UserProfile>(p => p.UserID);

        // 3. Một Unit có nhiều Nodes (Sửa: Đảm bảo Node có thuộc tính UnitId)
        modelBuilder.Entity<Node>()
            .HasOne<Unit>()
            .WithMany(u => u.Nodes)
            .HasForeignKey(n => n.UnitId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa Unit thì xóa luôn Nodes bên trong

        // 4. Một Node có nhiều Lessons (Sửa: Đảm bảo Lesson có thuộc tính NodeId)
        modelBuilder.Entity<Lesson>()
            .HasOne<Node>()
            .WithMany(n => n.Lessons)
            .HasForeignKey(l => l.NodeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 5. Index cho UserLessonProgress (Để một User không có 2 bản ghi tiến độ cho cùng 1 bài học)
        modelBuilder.Entity<UserLessonProgress>()
            .HasIndex(p => new { p.UserId, p.NodeId }).IsUnique();
    }
}