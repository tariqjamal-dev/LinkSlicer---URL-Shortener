using Microsoft.AspNetCore.Identity;

namespace LinkSlicer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? RegistrationDate { get; set; }
    }
}
