using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
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

            bool exists = Context.Followings.Any(f => f.FolloweeId == dto.FolloweeId && f.FollowerId == userId);

            if (exists)
                return BadRequest("You're already following this Artist!");

            // could also had check to prevent following yourself

            Following following = new Following()
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = userId
            };

            Context.Followings.Add(following);

            Context.SaveChanges();

            return Ok();
        }
    }
}
