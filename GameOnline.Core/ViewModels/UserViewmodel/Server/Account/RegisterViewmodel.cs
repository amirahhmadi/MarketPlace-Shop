using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.UserViewmodel.Server.Account
{
    public class RegisterViewmodel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
