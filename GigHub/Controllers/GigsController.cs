using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private ApplicationDbContext Context;

        public GigsController()
        {
            Context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Mine()
        {
            string userId = User.Identity.GetUserId();
            List<Gig> gigs = Context.Gigs
                .Where(g => g.ArtistId == userId && g.DateAdded > DateTime.Now && !g.IsCanceled)
                .Include(g => g.Genre)
                .ToList();

            return View(gigs);
        }

        [Authorize]
        public ActionResult Attending()
        {
            string userId = User.Identity.GetUserId();
            List<Gig> gigs = Context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToList();

            GigsViewModel viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };
            
            return View("Gigs", viewModel);
        }

        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        [Authorize]
        public ActionResult Create()
        {
            GigFormViewModel viewModel = new GigFormViewModel
            {
                Genres = Context.Genres.ToList(),
                Heading = "Add a Gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            // future: should not show buttons or give error if not user allowed to edit/delete
            string userId = User.Identity.GetUserId();

            Gig gig = Context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            GigFormViewModel viewModel = new GigFormViewModel
            {
                Id = gig.Id,
                Heading = "Edit a Gig",
                Genres = Context.Genres.ToList(),
                Date = gig.DateAdded.ToString("MM/dd/yyyy"),
                Time = gig.DateAdded.ToString("HH:mm"),
                Genre = gig.GenreId,
                Venue = gig.Venue
            };

            return View("GigForm", viewModel);
        }

        [Authorize] // requires user to be logged in for this method to be called
        [HttpPost] // can only be called in a form submit action
        [ValidateAntiForgeryToken] // used to prevent CSRF attacks. Used in conjuction with @Html.AntiForgeryToken() on view, this validates this token existed on the form that submitted this request.
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = Context.Genres.ToList();
                return View("GigForm", viewModel); // will return current model back to same view, which will keep fields populated and validation messages displayed
            }

            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateAdded = viewModel.GetDateAdded(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            gig.Create();

            Context.Gigs.Add(gig);
            Context.SaveChanges(); // writes to the database

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = Context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            string userId = User.Identity.GetUserId();

            Gig gig = Context.Gigs.Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Modify(viewModel.GetDateAdded(), viewModel.Venue, viewModel.Genre);

            Context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }
    }
}