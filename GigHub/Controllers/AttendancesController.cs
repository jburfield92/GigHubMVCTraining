using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers
{
    
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext Context;

        public AttendancesController()
        {
            Context = new ApplicationDbContext();
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            string userId = User.Identity.GetUserId();

            bool exists = Context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == dto.GigId);

            if (exists) return BadRequest("The attendance already exists.");

            Attendance attendance = new Attendance()
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            Context.Attendances.Add(attendance);
            Context.SaveChanges();

            return Ok();
        }
    }
}
