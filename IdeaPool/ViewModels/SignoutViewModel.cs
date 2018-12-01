using System.ComponentModel.DataAnnotations;

namespace MyIdeaPool.ViewModels
{
    public class SignoutViewModel
    {
        [Required] 
        public string refresh_token { get; set; }
    }
}