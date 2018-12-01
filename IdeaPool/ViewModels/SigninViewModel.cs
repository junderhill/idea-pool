using System.ComponentModel.DataAnnotations;

namespace MyIdeaPool.ViewModels
{
    public class SigninViewModel
    {
        [Required] 
        public string email { get; set; }
        
        [Required]
        public string password { get; set; }
    }
}