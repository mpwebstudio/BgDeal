using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Models
{
    public class Deal
    {
        private ICollection<Comment> comments;
        private ICollection<Vote> votes;
        private ICollection<Image> images;
        


        public Deal()
        {
            this.comments = new HashSet<Comment>();
            this.votes = new HashSet<Vote>();
            this.images = new HashSet<Image>();

        }

        //[Key]
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Location { get; set; }
        //[Required]
        public string AuthorId { get; set; }

        public int PotrebitelskiNomer { get; set; }
        public string Author { get; set; }

        public string HeadImage { get; set; }

        public string Phone { get; set; }

        public DateTime DateAdded { get; set; }

        public bool Active { get; set; }

        

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Vote> Votes
        {
            get { return this.votes; }
            set { this.votes = value; }
        }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }
    }
}
