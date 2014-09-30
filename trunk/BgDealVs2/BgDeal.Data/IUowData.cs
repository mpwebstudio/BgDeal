using BgDeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Data
{
    public interface IUowData
    {
        IRepository<Deal> Deals { get; }

        IRepository<Topic> Topics { get; }

        IRepository<Comment> Comments { get; }

        IRepository<Vote> Votes { get; }

        IRepository<Image> Images { get; }

        IRepository<DealUser> DealUsers { get; }

        IRepository<Message> Messages { get; }

        IRepository<MessageOut> MessagesOut { get; }

        int SaveChanges();
    }
}
