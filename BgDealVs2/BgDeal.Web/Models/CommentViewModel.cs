using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BgDeal.Web.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string AuthorUsername { get; set; }

        public string Content { get; set; }

        public DateTime DateAdded { get; set; }

        public int PotrebitelID { get; set; }

    }
}