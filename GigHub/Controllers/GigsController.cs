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

        [Authorize]
        [HttpPost]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            Gig gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateAdded = viewModel.DateAdded,
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };

            Context.Gigs.Add(gig);
            Context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}