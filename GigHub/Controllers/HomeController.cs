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

        public ActionResult Index(string query = null)
        {
            List<Gig> upcomingGigs = Context.Gigs
                .Include(gig => gig.Artist)
                .Include(gig => gig.Genre)
                .Where(gig => gig.DateAdded > DateTime.Now && !gig.IsCanceled)
                .ToList();

            if (!string.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs.Where(g => g.Artist.Name.Contains(query) || g.Genre.Name.Contains(query) || g.Venue.Contains(query)).ToList();
            }

            GigsViewModel viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Gigs",
                SearchTerm = query
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