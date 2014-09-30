using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BgDeal.Models;
using System.ComponentModel.DataAnnotations;


namespace BgDeal.Web.Models
{
    public class SubmitCommentModel
    {
        [Required]
        //Използваме вградената в MVC валидация
        //Коментара който потребителят праща за съответният лаптоп
        public string Comment { get; set; }
        //Тука получаваме hidden полето Html.Hidden("LaptopId", Model.Id) в details
        [Required]
        public int DealId { get; set; }

        public DateTime DateAdded { get; set; }

        public int PotrebitelID { get; set; }
    }
}