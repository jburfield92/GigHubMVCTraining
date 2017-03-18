using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class FolloweesController : Controller
    {
        private ApplicationDbContext Context;

        public FolloweesController()
        {
            Context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            List<ApplicationUser> followees = Context.Followings.Where(f => f.FollowerId == userId).Select(f => f.Followee).ToList();

            return View(followees);
        }
    }
}