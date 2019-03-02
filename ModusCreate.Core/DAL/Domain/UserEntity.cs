using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModusCreate.Core.DAL.Domain
{
    class UserEntity : IdentityUser
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public virtual ICollection<SubscriptionEntity> Subscriptions { get; set; }
    }
}
