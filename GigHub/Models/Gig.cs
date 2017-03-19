using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Models
{
    public class Gig
    {
        public int Id { get; private set; }

        public bool IsCanceled { get; private set; } // don't want someone to set the IsCanceled outside of calling the Cancel method since we want notifications to be sent always if the gig is canceled

        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public int GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            Notification notification = Notification.GigCanceled(this);

            foreach (ApplicationUser attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        public void Modify(DateTime dateAdded, string venue, int genreId)
        {
            Notification notification = Notification.GigUpdated(this, dateAdded, venue);
            
            Venue = venue;
            DateAdded = dateAdded;
            GenreId = genreId;

            foreach (ApplicationUser attendee in Attendances.Select(a => a.Attendee))
                attendee.Notify(notification);
        }
    }
}