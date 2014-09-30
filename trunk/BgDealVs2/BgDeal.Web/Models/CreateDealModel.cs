using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace BgDeal.Web.Models
{
    public class CreateDealModel
    {
        public int Id { get; set; }

        public string TopicId { get; set; }

        [Required(ErrorMessage = "Категорията е задължителна")]
        public string TopicSearch { get; set; }
        [Required(ErrorMessage = "Заглавието е задължително")]
        public string DealTitle { get; set; }
        [Required(ErrorMessage = "Местоположението е задължително")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Цената е задължителна")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Описанието е задължително")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Снимката е задължителна")]
        public string HeadImage { get; set; }

        public string Phone { get; set; }

        public DateTime DateAdded { get; set; }

        public string Author { get; set; }




        public SelectList DropDownList { get; set; }


    }

}