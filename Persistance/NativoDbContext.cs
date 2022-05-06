using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nativoshortener.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nativoshortener.api.Persistance
{
    public class NativoDbContext : IdentityDbContext
    {
        public NativoDbContext(DbContextOptions<NativoDbContext> options):base(options)
        {

        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShortenedUrl>().HasKey(s => s.Id);

            builder.Entity<Visit>()
                .HasOne(v => v.ShortenedUrl)
                .WithMany(s => s.Visits)
                .HasForeignKey(v => v.ShortenedUrlId);
        }
    }
}
