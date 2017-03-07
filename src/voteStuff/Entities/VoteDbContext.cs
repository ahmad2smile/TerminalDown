using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using voteStuff.Models;

namespace voteStuff.Entities
{
    public class VoteDbContext: IdentityDbContext<ApplicationUser>
    {
        public VoteDbContext(DbContextOptions options)
            :base(options)
        {
            
        }

        public DbSet<VoteDuo> VotesDb { get; set; }
        public DbSet<UserVotingDb> UserVotingDbs { get; set; }
        public DbSet<DuoVotedByUserDb> DuoVotedByUserDbs { get; set; }
    }
}
