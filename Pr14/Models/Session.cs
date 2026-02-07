using System;
using System.Collections.Generic;
using System.Text;

namespace Pr14.Models
{
    public class Session
    {
        public int Id { get; set; }

        public int FilmId { get; set; }

        public string SessionTime { get; set; }

        public string HallName { get; set; }

        public string HallType { get; set; }
    }
}
