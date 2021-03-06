﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

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
                .WithMany(g => g.Attendances) // each gig can have many attendances
                .WillCascadeOnDelete(false); // turns off cascade delete with the gig to the attendances

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers) // makes it so each user can have many followers
                .WithRequired(f => f.Followee) // each follower must have a followee
                .WillCascadeOnDelete(false); // turns off cascade delete with the followee to the followers

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followees) // makes it so each user can follow many followees
                .WithRequired(f => f.Follower) // each followee has a required follower
                .WillCascadeOnDelete(false); // turns off cascade delete with the follower to the followee

            modelBuilder.Entity<UserNotification>()
                .HasRequired(n => n.User) // makes it so eachuser notification can have a user
                .WithMany(u => u.UserNotifications) // each user can have many notifications
                .WillCascadeOnDelete(false); // turns off cascade delete with the UserNotification to the User

            base.OnModelCreating(modelBuilder);
        }
    }
}