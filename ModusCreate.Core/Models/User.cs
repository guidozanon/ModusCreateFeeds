using Microsoft.AspNetCore.Identity;

namespace ModusCreate.Core.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
    }
}
