using System;

namespace ModusCreate.Core.DAL.Domain
{
    class NewsEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual FeedEntity Feed { get; set; }
        public Guid FeedId { get; set; }
    }
}
