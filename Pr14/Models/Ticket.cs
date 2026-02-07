using System;
using System.Collections.Generic;
using System.Text;

namespace Pr14.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
    }
}
