using System;
using System.ComponentModel.DataAnnotations;
using BlimpBot.Constants;

namespace BlimpBot.Database.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ReviewCategory ReviewCategory{ get; set; }

        [Required] 
        [MaxLength(100)]
        public string Title{ get; set; }

        [Required]
        public int Rating{ get; set; }

        [MaxLength(250)]
        [Required]
        public string ShortText{ get; set; }

        [Required]
        public string LongText{ get; set; }

        [MaxLength(100)]
        public string Emoji{ get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
    }
}
