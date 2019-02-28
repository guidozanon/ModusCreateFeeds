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
    }
}
