using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BgDeal.Models;

namespace BgDeal.Web.Models
{
    public class ExtraInfoModel
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Image { get; set; }

        public virtual ApplicationUser Email { get; set; }

        public string Location { get; set; }

        public string Mobile { get; set; }
    }
}