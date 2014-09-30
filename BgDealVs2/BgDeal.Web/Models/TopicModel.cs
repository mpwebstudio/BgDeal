using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace BgDeal.Web.Models
{
    public class TopicModel
    {
        public int TopicId { get; set; }

        public IEnumerable<SelectListItem> Topic { get; set; }

    }
}