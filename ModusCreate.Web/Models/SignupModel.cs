using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ModusCreate.Web.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class SignupModel : LoginModel
    {
        [Required]
        public string Name { get; set; }

    }

    public static class GravatarHelper
    {
        private const string BaseUrl = "https://www.gravatar.com/avatar/{0}";

        public static string GenerateImageUrl(this string email)
        {
            using (var hasher = MD5.Create())
            {
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(email.Trim().ToLower()));

                return string.Format(BaseUrl, string.Concat(hash.Select(b => b.ToString("X2")))).ToLower();
            }
        }
    }
}
