using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Common;
using Web_ASPMVC.Models;

namespace Web_ASPMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            /*  ViewBag.slides = new SildeDAO().ListAll(); *///thêm slide chuyển động(chưa hoàn thành được)
            var productDAO = new ProductDAO();
            ViewBag.ListRow1 = productDAO.ListRow1(4);
            ViewBag.ListRow2 = productDAO.ListRow2(4);

         
            return View();
        }
       [ChildActionOnly]//chỉ được gọi thông qua Action, không gọi trực tiếp được
       public ActionResult MainMenu()
        {
            var model = new MenuDAO().ListByGroupID(1);
            return PartialView(model);
        }
        [ChildActionOnly]//chỉ được gọi thông qua Action, không gọi trực tiếp được
        public ActionResult TopMenu()
        {
            var model = new MenuDAO().ListByGroupID(2);
            return PartialView(model);
        }
        [ChildActionOnly]//chỉ được gọi thông qua Action, không gọi trực tiếp được
        public ActionResult Footer()
        {
            var model = new FooterDAO().GetFooter();
            return PartialView(model);
        }
        /// <summary>
        /// hiện thị số lượng hàng đã mua
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult HeaderCart() 
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if(cart !=null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}