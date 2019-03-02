using System;
using System.Collections.Generic;
using System.Text;

namespace ModusCreate.Core.DAL.Domain
{
    class SubscriptionEntity
    {
        public virtual UserEntity User { get; set; }
        public virtual FeedEntity Feed { get; set; }
        public Guid FeedId { get; set; }
        public string UserId { get; set; }
    }
}
