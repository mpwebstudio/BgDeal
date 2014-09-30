using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BgDeal.Web.Models
{
    public class DealViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public bool Active { get; set; }

        public string Author { get; set; }



    }
}