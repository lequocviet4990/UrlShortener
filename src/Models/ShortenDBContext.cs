

using Microsoft.EntityFrameworkCore;

namespace ShortLinkApi.Models
{
    public class ShortenDBContext : DbContext
    {
        public ShortenDBContext(DbContextOptions<ShortenDBContext> options): base(options)  
        {

        }

        public DbSet<ShortLink> ShortLinks {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        

    }
}