using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgDeal.Models
{
    public class MessageOut
    {
        public int Id { get; set; }

        public string MessageFrom { get; set; }

        public int AuthorId { get; set; }

        public string MessageTo { get; set; }

        public int MessageToId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateSend { get; set; }

        public int DealId { get; set; }

        public bool MessageRead { get; set; }

        public string DealTitle { get; set; }
    }
}
