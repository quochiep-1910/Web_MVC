using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public JsonResult Update(string cartModel)
        {
            var Jsoncart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel); //chuyển 1 chuỗi json thành định dạng CartItem(model)
            var sessionCart = (List<CartItem>)Session[CartSession]; //gán session
            foreach (var item in sessionCart)
            {
                var jsonItem = Jsoncart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if(jsonItem !=null)
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
    }
}