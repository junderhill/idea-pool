using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyIdeaPool.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(128)]
        public string Fullname { get; set; }

        [MaxLength(40)]
        public string RefreshToken { get; set; }
    }
}
