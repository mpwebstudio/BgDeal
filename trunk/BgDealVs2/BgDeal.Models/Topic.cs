using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Models
{
    public class Topic
    {

        private ICollection<Deal> deals;

        public Topic()
        {
            this.deals = new HashSet<Deal>();
        }


        public int Id { get; set; }


        public string Name { get; set; }

        public virtual ICollection<Deal> Deal
        {
            get { return this.deals; }
            set { this.deals = value; }
        }

        public IEnumerable<Topic> Topics { get; set; }
    }
}
