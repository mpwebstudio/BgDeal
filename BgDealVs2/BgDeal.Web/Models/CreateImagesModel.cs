using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BgDeal.Web.Models
{
    public class CreateImagesModel
    {
        public int Id { get; set; }

        public string ImageName { get; set; }

        public int DealId { get; set; }
    }
}