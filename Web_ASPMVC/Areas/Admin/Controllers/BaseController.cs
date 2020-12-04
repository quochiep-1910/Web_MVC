using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Common;
using System.Web.Routing;
namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        //protected là tất cả các lớp nào kế thừa từ base đều có thể dùng
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        // xữ lý ngoại lệ khi người dùng đăng nhập vào Home bỏ qua đăng nhập
        //ghi đè không cho người dùng vào thằng Home, không thông qua đăng nhập, bắt lỗi người dùng
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];//kiểm tra session và ép kiểu sang userlogin
            if (session == null)//nếu session bằng null tức là không truyền được 'USER_SESSION'
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index", Areas = "admin" }));//sẽ trở về login với index
            }
            base.OnActionExecuted(filterContext);
        }
        protected void SetAlert(string message, string type)
        {
            //1 đối tượng có thể tag từ server về
            TempData["AlertMessage"] = message; //gán key
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";//alert-success là class của bootstrap
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}