using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace voteStuff.Models
{
    public class RegisterViewModel
    {
        [Required, StringLength(maximumLength:50,MinimumLength = 5)]
        public string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password)), DisplayName(displayName: "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}