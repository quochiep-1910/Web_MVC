using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using Web_ASPMVC.Common;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        /// <summary>
        /// Đa ngôn ngữ
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session[CommonConstants.CurrentCulture] != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session[CommonConstants.CurrentCulture].ToString());
            }
            else
            {
                Session[CommonConstants.CurrentCulture] = "vi";
                Thread.CurrentThread.CurrentCulture = new CultureInfo("vi");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi");
            }
        }

        /// <summary>
        /// Đa ngôn ngữ
        /// </summary>
        /// <param name="ddlCulture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        // changing culture
        public ActionResult ChangeCulture(string ddlCulture, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ddlCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(ddlCulture);

            Session[CommonConstants.CurrentCulture] = ddlCulture;
            return Redirect(returnUrl);
        }

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