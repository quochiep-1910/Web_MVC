using Common;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Web_ASPMVC
{
    //Xác thực xem trong trang admin thì tài khoản nào được quyền coi và tài khoản nào được quyền sửa xoá
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (Common.UserLogin)HttpContext.Current.Session[Common.CommonConstants.USER_SESSION]; //lấy ra id group
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(session.UserName); // Gọi một phương thức khác để nhận quyền của người dùng từ DB

            if (privilegeLevels.Contains(this.RoleID) || session.GroupID == CommonConstants.ADMIN_GROUP) //kiểm tra xem USER này có quyền gì và nếu là admin thì toàn quyền
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ghi đè lên khi phát hiện lỗi và show lên trang lỗi
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
            };
        }

        /// <summary>
        /// Nhận thông tin xác thực bằng người dùng đã đăng nhập
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private List<string> GetCredentialByLoggedInUser(string userName)
        {
            //giống như tạo 1 cái giấy thông hành
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstants.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}