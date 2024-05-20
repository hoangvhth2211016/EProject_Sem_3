using EProject_Sem_3.Models.BookImages;
using EProject_Sem_3.Models.Books;
using EProject_Sem_3.Models.Feedbacks;
using EProject_Sem_3.Models.OrderDetails;
using EProject_Sem_3.Models.Orders;
using EProject_Sem_3.Models.Plans;
using EProject_Sem_3.Models.RecipeImages;
using EProject_Sem_3.Models.Recipes;
using EProject_Sem_3.Models.Subscriptions;
using EProject_Sem_3.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace EProject_Sem_3.Models;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        // for user
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasData(
            new User {
                Id = 1,
                Username = "admin",
                Name = "admin",
                Email = "admin@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                Role = Role.Admin,
                IsActivated = true
            }
            );
        
        // for plans
        modelBuilder.Entity<Plan>().HasData(
            new Plan {
                Id = 1,
                Price = 15,
                Type = PlanType.Monthly
            },
            new Plan {
                Id = 2,
                Price = 150,
                Type = PlanType.Yearly
            }
            );
        
        base.OnModelCreating(modelBuilder);
        
        // When deleting an order, the corresponding orderDetail will be deleted
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderDetails)
            .WithOne(od => od.Order)
            .OnDelete(DeleteBehavior.Cascade);
        
        // When deleting an book, the corresponding image book will be deleted
        modelBuilder.Entity<Book>()
            .HasMany(o => o.BookImages)
            .WithOne(od => od.Book)
            .OnDelete(DeleteBehavior.Cascade);
    }
    

    public DbSet<User> Users { get; set; }

    public DbSet<Plan> Plans { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<Recipe> Recipes { get; set; }

    public DbSet<RecipeImage> RecipeImages { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<BookImage> BookImages { get; set; }

    public override int SaveChanges() {
        UpdateTimeStamp();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        UpdateTimeStamp();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimeStamp() {
        var currentTime = DateTime.Now;

        foreach (var entry in ChangeTracker.Entries()) {
            if (entry.State == EntityState.Added) {
                if (entry.Entity is BaseEntity entity) {
                    entity.CreatedAt = currentTime;
                    entity.UpdatedAt = currentTime;
                }
            }
            else if (entry.State == EntityState.Modified) {
                if (entry.Entity is BaseEntity entity) {
                    entity.UpdatedAt = currentTime;
                }
            }
        }
    }
}