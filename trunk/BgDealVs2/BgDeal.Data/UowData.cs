using BgDeal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Data
{
    public class UowData : IUowData
    {
        private readonly DbContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();


        public UowData()
            : this(new ApplicationDbContext())
        {
        }

        public UowData(DbContext context)
        {
            this.context = context;
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IRepository<Deal> Deals
        {
            get { return this.GetRepository<Deal>(); }
        }

        public IRepository<Topic> Topics
        {
            get { return this.GetRepository<Topic>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<Vote> Votes
        {
            get { return this.GetRepository<Vote>(); }
        }

        public IRepository<Image> Images
        {
            get { return this.GetRepository<Image>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public IRepository<MessageOut> MessagesOut
        {
            get { return this.GetRepository<MessageOut>(); }
        }

        public IRepository<DealUser> DealUsers
        {
            get { return this.GetRepository<DealUser>(); }
        }
    }
}
