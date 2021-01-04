using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string search, int page = 1, int pageSize = 4)
        {
            var dao = new ProductDAO();
            var model = dao.ListAllPaging(search, page, pageSize);//truyền page và pageSize vàoo
            ViewBag.search = search;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new ProductDAO();
                long id = dao.Insert(product); //gán biến id vào DAO
                if (id > 0) //nếu insert thành công thì id>0
                {
                    SetAlert("Thêm danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm danh mục sản phẩm thất bại");
                }
            }
            return View("Index");
        }
    }
}