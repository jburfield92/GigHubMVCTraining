using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
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