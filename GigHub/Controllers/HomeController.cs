using GigHub.Models;
using GigHub.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext Context;

        public HomeController()
        {
            Context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            List<Gig> upcomingGigs = Context.Gigs
                .Include(gig => gig.Artist)
                .Include(gig => gig.Genre)
                .Where(gig => gig.DateAdded > DateTime.Now).ToList();

            GigsViewModel viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs"
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}