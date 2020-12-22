using System.ComponentModel.DataAnnotations;

namespace Web_ASPMVC.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời Nhập UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời Nhập Password")]
        public string Password { get; set; }

        public bool RemeberMe { get; set; }
    }
}