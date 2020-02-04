using Microsoft.AspNetCore.Identity;

namespace Identity.WebApi.Models
{
    public class User : IdentityUser
    {
        public int AccountNumber { get; set; }
        public string FullName { get; set; }
        public string Currency { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
