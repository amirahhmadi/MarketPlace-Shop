using System.ComponentModel.DataAnnotations;

namespace GameOnline.Core.ViewModels.UserViewmodel.Server.Account
{
    public class LoginViewmodel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
