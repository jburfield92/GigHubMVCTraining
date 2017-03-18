using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext Context;

        public GigsController()
        {
            Context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            string userId = User.Identity.GetUserId();
            List<Gig> gigs = Context.Attendances.Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre).ToList();

            GigsViewModel viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };
            
            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            GigFormViewModel viewModel = new GigFormViewModel
            {
                Genres = Context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize] // requires user to be logged in for this method to be called
        [HttpPost] // can only be called in a form submit action
        [ValidateAntiForgeryToken] // used to prevent CSRF attacks. Used in conjuction with @Html.AntiForgeryToken() on view, this validates this token existed on the form that submitted this request.
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = Context.Genres.ToList();
                return View("Create", viewModel); // will return current model back to same view, which will keep fields populated and validation messages displayed
            }

            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateAdded = viewModel.GetDateAdded(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            Context.Gigs.Add(gig);
            Context.SaveChanges(); // writes to the database

            return RedirectToAction("Index", "Home");
        }
    }
}