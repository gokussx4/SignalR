using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingAPI.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime When { get; set; }

        public int UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

    public class MessageBoardMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime When { get; set; }

        public string Username { get; set; }
    }
}
