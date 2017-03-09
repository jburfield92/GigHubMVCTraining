using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }


        public ApplicationDbContext(): base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.Gig) // makes it so each attendance have a required gig
                .WithMany() // each gig can have many attendances
                .WillCascadeOnDelete(false); // turns off cascade delete with the gig to the attendances

            modelBuilder.Entity<Following>()
                .HasRequired(a => a.Artist) // makes it so each following have a required artist (the followed)
                .WithMany() // each artist can have many followers
                .WillCascadeOnDelete(false); // turns off cascade delete with the artist to the followers

            base.OnModelCreating(modelBuilder);
        }
    }
}