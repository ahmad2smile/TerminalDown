using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace voteStuff.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FbUserId { get; set; }
    }
}
