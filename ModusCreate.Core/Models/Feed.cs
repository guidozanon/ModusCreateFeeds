using System;
using System.Collections.Generic;

namespace ModusCreate.Core.Models
{
    public class Feed
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public bool IsSubscribed { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
