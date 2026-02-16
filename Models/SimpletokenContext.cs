using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SimpleTokenGenerate.Models;

public partial class SimpletokenContext : DbContext
{
    public SimpletokenContext()
    {
    }

    public SimpletokenContext(DbContextOptions<SimpletokenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       // => optionsBuilder.UseMySQL("server=localhost;database=simpletoken;user=root;password=");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Password).HasMaxLength(60);
            entity.Property(e => e.UserName).HasMaxLength(40);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
