using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyIdeaPool.Models
{
    public class Idea
    {
        public Idea()
        {
            CreatedTimestamp = DateTime.UtcNow;
        }
        
        [Key]
        public Guid Id { get; set; }
        
        [MaxLength(255)]
        [Required]
        public string Content { get; set; }
        
        [Required]
        public int Impact { get; set; }
        [Required]
        public int Ease { get; set; }
        [Required]
        public int Confidence { get; set; }
      
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public double Average
        {
            get
            {
                double total = Impact + Ease + Confidence;
                return total / 3;
            }
        }
        
        public DateTime CreatedTimestamp { get; }
    }
}