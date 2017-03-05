using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace voteStuff.Models
{
    public class LoginViewModel
    {
        [Required, StringLength(maximumLength: 50, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName(displayName:"Remember Me?")]
        public bool RememberMe { get; set; }
    }
}