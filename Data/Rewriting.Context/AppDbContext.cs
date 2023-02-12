using Microsoft.EntityFrameworkCore;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Context;

public class AppDbContext : DbContext
{
    public DbSet<UserData> UsersData { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Order> Orders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(DbConstants.DATABASE_SCHEME);

        modelBuilder.Entity<UserData>()
            .ToTable("users_data")
            ;

        modelBuilder.Entity<Offer>()
            .ToTable("offers")
            .HasOne(t => t.Order).WithMany(s => s.Offers)
            .OnDelete(DeleteBehavior.Restrict)
            ;
        modelBuilder.Entity<Offer>()
            .HasOne(t => t.Contractor).WithMany(s => s.Offers)
            .OnDelete(DeleteBehavior.Restrict)
            ;

        modelBuilder.Entity<Order>()
            .ToTable("order")
            .HasOne(t => t.Client).WithMany(t => t.Orders)
            .OnDelete(DeleteBehavior.Restrict)
            ;
        modelBuilder.Entity<Order>()
            .HasOne(t => t.Contractor).WithMany(t => t.Contracts)
            .OnDelete(DeleteBehavior.Restrict)
            ;


    }
}
