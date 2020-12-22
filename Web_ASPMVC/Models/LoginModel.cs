namespace Web_ASPMVC.Models
{
    public class LoginModel
    {
        //[Display(Name = "Tên Đăng Nhập")]
        //[Required(ErrorMessage = "Bạn phải nhập tài khoản")]
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}