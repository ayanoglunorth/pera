using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pera.Entity.Entities
{
    [Table("Messages")]
    public class Message
    {
        public int Id { get; set; }

        // From?
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        // To?
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

        public string Content { get; set; }
        public DateTime Date { get; set; }

        // Read receipt (blue tick) logic
        public bool IsRead { get; set; } = false;
    }
}
