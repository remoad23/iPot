using System;
using System.ComponentModel.DataAnnotations;

namespace iPotAPI.Model
{
    public class PourTime
    {
        [Key]
        public int PourTimeId { get; set; }
        public int percentageToPour { get; set; }
        public string timeToPour { get; set; }
    }
}