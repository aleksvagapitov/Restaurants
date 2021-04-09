using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; }
        public DbSet<RestaurantHours> RestaurantHours { get; set; }
        public DbSet<UserFollowing> Followings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Used for Identity

            builder.HasPostgresExtension("postgis");
        
            builder.Entity<AppUser>(b => {
                b.HasMany(p => p.Photos)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
                
                b.HasMany(r => r.Reviews)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.Restaurants)
                    .WithOne()
                    .OnDelete(DeleteBehavior.NoAction);
            });
 
            builder.Entity<Restaurant>(b => 
            {   
                b.HasMany(c => c.Categories);

                b.HasMany(c => c.WorkHours);

                b.HasMany(r => r.Reviews)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(c => c.Photos)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserFollowing>(b =>
            {
                b.HasKey(k => new {k.ObserverId, k.TargetId});
                
                b.HasOne(o => o.Observer)
                    .WithMany(f => f.Followings)
                    .HasForeignKey(o => o.ObserverId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(o => o.Target)
                    .WithMany(f => f.Followers)
                    .HasForeignKey(o => o.TargetId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}