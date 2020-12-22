using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_ASPMVC.Models;

namespace Web_ASPMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: GioHang
        /// <summary>
        /// key lưu session
        /// </summary>
        private const string CartSession = "CartSession";

        public ActionResult Index()
        {
            var cart = Session[CartSession]; //khởi tạo
            var list = new List<CartItem>(); //lấy đối tượng truyền từ AddItem
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(long producId, int quantity)
        {
            var product = new ProductDAO().ViewDetail(producId); // lấy ra thông tin sản phẩm
            var cart = Session[CartSession];
            if (cart != null) //nếu cart != null tức là đã có sản phẩm trong giỏ hàng rồi, cần tăng số lượng lên
            {
                var list = (List<CartItem>)cart; //ép kiểu sang CartItem
                if (list.Exists(x => x.Product.ID == producId)) //
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == producId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else //thêm mới vào giỏ hàng
            {
                //tạo mới đối tượng
                var item = new CartItem(); //khởi tạo
                item.Product = product; //gán product
                item.Quantity = quantity; //gán Quantity
                //gán đối tượng vào list và gán cho session
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list; //gán session cho list
            }
            return RedirectToAction("Index");
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession]; //lấy danh sách giỏ hàng gán session
            sessionCart.RemoveAll(x => x.Product.ID == id); //remove những cái cardItem nào mà có cái productID = id truyền vào
            Session[CartSession] = sessionCart; //gán lại
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Update(string cartModel)
        {
            var Jsoncart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel); //chuyển 1 chuỗi json thành định dạng CartItem(model)
            var sessionCart = (List<CartItem>)Session[CartSession]; //lấy ra List(CartItem) gán session
            foreach (var item in sessionCart)
            {
                var jsonItem = Jsoncart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession]; //lấy danh sách
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string ShipName, string mobile, string address, string email)
        {
            //lấy thông tin truyền vào order
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipEmail = email;
            order.ShipName = ShipName;
            try
            {
                //lấy thông tin
                var id = new OrderDAO().Insert(order);//lấy ad sản phẩm
                var cart = (List<CartItem>)Session[CartSession]; //lấy ra thông tin sản phẩm
                var detailDao = new OrderDetailsDAO();
                decimal total = 0;
                foreach (var item in cart)
                {
                    //insert vào OrderDetail
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity); //tổng tiền
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));//định đạng thành đối tượng
                content = content.Replace("{{CustomerName}}", ShipName); //lấy giá trị CustomerName từ file neworder.html
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0")); //ToString("N0"): chuyển đổi giá trị sang VN
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString(); //mail quản trị
                //Gửi mail thông báo cho người dùng và quản trị có đơn hàng mới
                new MailHelp().SendMail(email, "Đơn hàng mới từ GROCERY STORE", content);//gửi tới mail khách hàng
                new MailHelp().SendMail(toEmail, "Đơn hàng mới từ GROCERY STORE", content); //mail này trả ngược về mail admin
            }
            catch (Exception ex)
            {
                //ghi log
                return Redirect("/loi-thanh-toan");
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}