
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BgDeal.Web.Models
{
    public class PublicProfile
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Location { get; set; }

        public string Mobile { get; set; }

        public int DealPost { get; set; }

        public string DateRegister { get; set; }

        public string HeadPicture { get; set; }

        public string Author { get; set; }

        

        public IEnumerable<DealViewModel> Deals { get; set; }
 
        
    }
}