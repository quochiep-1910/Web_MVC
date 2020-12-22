using Models.DAO;
using Models.EF;
using System.Web.Mvc;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string search, int page = 1, int pageSize = 4)
        {
            var dao = new UserDAO();
            var model = dao.ListAllPaging(search, page, pageSize); //truyền page và pageSize vàoo
            ViewBag.search = search;
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)//kiểm tra rỗng
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

        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDAO().ViewDetail(id);
            return View(user);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
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
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new UserDAO().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
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