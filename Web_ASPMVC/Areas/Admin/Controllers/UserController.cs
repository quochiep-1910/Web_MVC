using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string search, int page = 1, int pageSize =10 )
        {
            var dao = new UserDAO();
            var model = dao.ListAllPaging(search, page, pageSize); //truyền page và pageSize vàoo
            ViewBag.search = search;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if(ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new UserDAO();
                long id = dao.Insert(user); //gán biến id vào DAO
                if (id > 0) //nếu insert thành công thì id>0
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user thất bại");
                }
            }
            return View("Index");

        }
        public ActionResult Edit(int id)
        {
            var user = new UserDAO().ViewDetail(id);
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new UserDAO();
                var result = dao.Update(user); //gán biến id vào DAO
                if (result) //nếu insert thành công thì id>0
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "User");
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
            new UserDAO().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id) //id trong Models.EF.User truyền kiểu gì thì truyền y như vậy
        {
            var result = new UserDAO().ChangeStatus(id);
            return Json(new
            {
                status = result  //trả về đối tượng json
            });
        }
    }
}