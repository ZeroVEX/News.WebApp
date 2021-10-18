using NewsApp.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Repositories.Contexts
{
    public sealed class NewsDbContext : DbContext
    {
        public NewsDbContext(DbContextOptions<NewsDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(u => u.Email).IsUnique();

                b.Property(u => u.Email).HasMaxLength(User.EmailMaxLength).IsRequired();
                b.Property(u => u.NormalizedEmail).HasMaxLength(User.NormalizedEmailMaxLength).IsRequired();
                b.Property(u => u.DisplayName).HasMaxLength(User.DisplayNameMaxLength).IsRequired();
                b.Property(u => u.PasswordHash).IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasIndex(r => r.NormalizedName).IsUnique();

                b.Property(r => r.Name).HasMaxLength(Role.NameMaxLength).IsRequired();
                b.Property(r => r.NormalizedName).HasMaxLength(Role.NormalizedNameMaxLength).IsRequired();

                b.HasData(
                    new Role
                    {
                        Id = 1,
                        Name = RoleNames.Admin,
                        NormalizedName = RoleNames.Admin.ToUpper()
                    },
                    new Role
                    {
                        Id = 2,
                        Name = RoleNames.User,
                        NormalizedName = RoleNames.User.ToUpper()
                    });
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
            });

            modelBuilder.Entity<News>(b =>
            {
                b.Property(n => n.Title).HasMaxLength(News.TitleMaxLength).IsRequired();
                b.Property(n => n.Subtitle).HasMaxLength(News.SubtitleMaxLength).IsRequired();
                b.Property(n => n.Text).HasMaxLength(News.TextMaxLength).IsRequired();
            });
        }
    }
}