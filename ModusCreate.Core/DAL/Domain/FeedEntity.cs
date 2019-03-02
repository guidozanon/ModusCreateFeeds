using System;
using System.Collections.Generic;

namespace ModusCreate.Core.DAL.Domain
{
    class FeedEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual FeedCategoryEntity Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }

        public virtual ICollection<NewsEntity> News { get; set; }
        public virtual ICollection<SubscriptionEntity> Subscriptions { get; set; }
    }
}
