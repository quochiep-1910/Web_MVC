using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string search, int page = 1, int pageSize = 4)
        {
            var dao = new ProductCategoryDAO();
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
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new ProductCategoryDAO();
                long id = dao.Insert(productCategory); //gán biến id vào DAO
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

        public ActionResult Edit(int id)
        {
            var user = new ProductCategoryDAO().ViewDetail(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new ProductCategoryDAO();
                var result = dao.Update(productCategory); //gán biến id vào DAO
                if (result) //nếu insert thành công thì id>0
                {
                    SetAlert("Sửa danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhập thất bại");
                }
            }
            return View("Index");
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductCategoryDAO().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id) //id trong Models.EF.User truyền kiểu gì thì truyền y như vậy
        {
            var result = new ProductCategoryDAO().ChangeStatus(id);
            return Json(new
            {
                status = result  //trả về đối tượng json
            });
        }
    }
}