using Microsoft.AspNetCore.Identity;

namespace ContactBook.Model
{
    public class User : IdentityUser
    {
        public string? ImageUrl { get; set; }
    }
}
