using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace voteStuff.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FbUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
