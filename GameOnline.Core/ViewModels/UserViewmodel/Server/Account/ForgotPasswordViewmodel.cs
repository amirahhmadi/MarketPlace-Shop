using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.UserViewmodel.Server.Account
{
    public class ForgotPasswordViewmodel
    {
        public int UserId { get; set; }
        public string ActiveCode { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
