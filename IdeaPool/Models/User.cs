using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyIdeaPool.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Fullname { get; set; }
    }
}