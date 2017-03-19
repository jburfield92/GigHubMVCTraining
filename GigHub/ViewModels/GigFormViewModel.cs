using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Venue { get; set; }

        [Required]
        [FutureDate] // other attributes to use: stringlength, range, etc.
        public string Date { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }
        
        [Required]
        public int Genre { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<GigsController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<GigsController, ActionResult>> create = (c => c.Create(this));
                Expression<Func<GigsController, ActionResult>> action = (Id != 0) ? update : create;

                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
        
        public IEnumerable<Genre> Genres { get; set; }

        public DateTime GetDateAdded()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}