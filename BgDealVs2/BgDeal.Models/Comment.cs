using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int PotrebitelID { get; set; }

        public int DealId { get; set; }

        public virtual Deal Deal { get; set; }


        public string Content { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
