using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BgDeal.Web.Models
{
    public class DetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public decimal Price { get; set; }

        public string Location { get; set; }

        public string HeadImage { get; set; }

        public DateTime DateAdded { get; set; }

        public string Author { get; set; }

        public int AuthorId { get; set; }

        public IEnumerable<CreateImagesModel> Images { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public string Phone { get; set; }

        public string MessageTitle { get; set; }

        public string MessageContent { get; set; }

        public int CurrentPage { get; set; }


    }
}