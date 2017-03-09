using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    public class FollowingController : ApiController
    {
        private ApplicationDbContext Context;

        public FollowingController()
        {
            Context = new ApplicationDbContext();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            string userId = User.Identity.GetUserId();

            bool exists = Context.Followings.Any(f => f.ArtistId == dto.ArtistId && f.FollowerId == userId);

            if (exists)
                return BadRequest("You're already following this Artist!");

            Following following = new Following()
            {
                ArtistId = dto.ArtistId,
                FollowerId = userId
            };

            Context.Followings.Add(following);

            Context.SaveChanges();

            return Ok();
        }
    }
}
