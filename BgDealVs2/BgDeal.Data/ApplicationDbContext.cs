using BgDeal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IDbSet<Deal> Deals { get; set; }

        public IDbSet<Topic> Topics { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Vote> Votes { get; set; }

        public IDbSet<DealUser> DealUsers { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<MessageOut> MessagesOut { get; set; }

        public IDbSet<Image> Images { get; set; }
    }
}
