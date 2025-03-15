using LinkSlicer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkSlicer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }

    }
}
