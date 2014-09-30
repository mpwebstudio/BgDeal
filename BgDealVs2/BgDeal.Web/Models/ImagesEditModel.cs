using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BgDeal.Web.Models
{
    public class ImagesEditModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public int DealId { get; set; }
    }
}