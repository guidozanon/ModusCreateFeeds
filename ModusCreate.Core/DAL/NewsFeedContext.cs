using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModusCreate.Core.DAL.Domain;

namespace ModusCreate.Core.DAL
{
    class NewsFeedContext : IdentityDbContext<UserEntity>
    {
        public NewsFeedContext(DbContextOptions<NewsFeedContext> options) : base(options)
        {

        }

        public virtual DbSet<FeedEntity> Feeds { get; set; }
        public virtual DbSet<NewsEntity> News { get; set; }
        public virtual DbSet<UserTokenEntity> UserJwtTokens { get; set; }
        public virtual DbSet<FeedCategoryEntity> Categories { get; set; }
        public virtual DbSet<SubscriptionEntity> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SubscriptionEntity>()
                .HasKey(t => new { t.UserId, t.FeedId });

            builder.Entity<SubscriptionEntity>()
                .HasOne(pt => pt.Feed)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(pt => pt.FeedId);

            builder.Entity<SubscriptionEntity>()
                .HasOne(pt => pt.User)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(pt => pt.UserId);

            base.OnModelCreating(builder);


        }
    }
}
