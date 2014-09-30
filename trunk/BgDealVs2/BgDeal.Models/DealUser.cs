using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Models
{
    public class DealUser
    {
        private ICollection<Deal> deals;

        public DealUser()
        {
            this.deals = new HashSet<Deal>();
        }
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Author { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Image { get; set; }

        public virtual ApplicationUser Email { get; set; }

        public string Location { get; set; }

        public string Mobile { get; set; }

        public string DateRgister { get; set; }

        public virtual ICollection<Deal> Deals
        {
            get { return this.deals; }
            set { this.deals = value; }
        }
    }
}
