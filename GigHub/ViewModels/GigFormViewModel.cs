using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
                return Id != 0 ? "Update" : "Create";
            }
        }
        
        public IEnumerable<Genre> Genres { get; set; }

        public DateTime GetDateAdded()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}