using System.ComponentModel.DataAnnotations;

namespace Web_ASPMVC.Models
{
    public class RegisterModel
    {
        [Display(Name = "Tên Đăng Nhập")]
        //[Required(ErrorMessage ="Yêu cầu nhập tên đăng nhập")] //đã có nên bỏ qua
        public string UserName { get; set; }

        [Display(Name = "Mật Khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mặt khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng.")] //so sánh mật khẩu
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ tên")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Tỉnh/thành")]
        public string ProvinceID { set; get; }

        [Display(Name = "Quận/Huyện")]
        public string DistrictID { set; get; }

        public string CaptchaCode { get; set; }
    }
}