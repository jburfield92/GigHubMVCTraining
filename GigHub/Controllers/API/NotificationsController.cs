using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext Context;

        public NotificationsController()
        {
            Context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            string userId = User.Identity.GetUserId();

            // don't return domain objects to client via web APIs, use DTOs instead to modify the data returned
            List<Notification> notifications = Context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            return notifications.Select(n => new NotificationDto()
            {
                DateTime = n.DateTime,
                Gig = new GigDto
                {
                    Artist = new UserDto
                    {
                        Id = n.Gig.Artist.Id,
                        Name = n.Gig.Artist.Name
                    },
                    DateAdded = n.Gig.DateAdded,
                    Id = n.Gig.Id,
                    IsCanceled = n.Gig.IsCanceled,
                    Venue = n.Gig.Venue
                },
                OriginalDateTime = n.OriginalDateTime,
                OriginalVenue = n.OriginalVenue,
                Type = n.Type
            });

        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            string userId = User.Identity.GetUserId();

            List<UserNotification> notifications = Context.UserNotifications.Where(un => un.UserId == userId && !un.IsRead).ToList();

            notifications.ForEach(n => n.Read());

            Context.SaveChanges();

            return Ok();
        }
    }
}
