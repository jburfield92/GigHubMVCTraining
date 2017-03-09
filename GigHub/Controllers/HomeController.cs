using GigHub.Models;
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

            return View(upcomingGigs);
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