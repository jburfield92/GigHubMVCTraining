using System;

namespace GigHub.DTOs
{
    public class GigDto
    {
        public int Id { get; set; } // used for link to find the gig
        public bool IsCanceled { get; set; }
        public UserDto Artist { get; set; }
        public DateTime DateAdded { get; set; }
        public string Venue { get; set; }
        public GenreDto Genre { get; set; }
    }
}