using System;

namespace ModusCreate.Core.Models
{
    public class News
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
