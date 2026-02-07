using System;
using System.Collections.Generic;
using System.Text;

namespace Pr14.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string PreviewFilePath { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public string StartDate { get; set; }
        public string AgeRating { get; set; }
        public string ShortDescription { get; set; }
        public string Genres { get; set; }
        public Decimal PriceForBasic { get; set; }
        public Decimal PriceForVip { get; set; }
    }
}
