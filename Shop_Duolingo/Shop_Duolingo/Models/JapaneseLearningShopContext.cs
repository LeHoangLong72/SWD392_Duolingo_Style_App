using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Shop_Duolingo.Models;

public partial class JapaneseLearningShopContext : DbContext
{
    public JapaneseLearningShopContext()
    {
    }

    public JapaneseLearningShopContext(DbContextOptions<JapaneseLearningShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserItem> UserItems { get; set; }

    public virtual DbSet<VwPopularItem> VwPopularItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Items__3214EC072215A2EC");

            entity.HasIndex(e => e.Category, "IX_Items_Category");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DescriptionVi).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameVi).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC072020E872");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4048D08F1").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534ECC8A86A").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserItem__3214EC07EFFB3DF6");

            entity.HasIndex(e => e.ItemId, "IX_UserItems_ItemId");

            entity.HasIndex(e => e.UserId, "IX_UserItems_UserId");

            entity.HasIndex(e => new { e.UserId, e.ItemId }, "UQ_UserItems_UserId_ItemId").IsUnique();

            entity.Property(e => e.PurchasedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Item).WithMany(p => p.UserItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK_UserItems_Items");

            entity.HasOne(d => d.User).WithMany(p => p.UserItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserItems_Users");
        });

        modelBuilder.Entity<VwPopularItem>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PopularItems");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameVi).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
