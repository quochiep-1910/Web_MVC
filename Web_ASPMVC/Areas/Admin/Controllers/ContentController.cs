using Models.DAO;
using Models.EF;
using System.Web.Mvc;
using Web_ASPMVC.Common;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(string search, int page = 1, int pageSize = 4)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(search, page, pageSize); //truyền page và pageSize vàoo
            ViewBag.search = search;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]//Thuộc tính ValidateInput (false) được sử dụng để cho phép gửi nội dung hoặc mã HTML đến máy chủ
        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                var culture = Session[CommonConstants.CurrentCulture];
                model.Language = culture.ToString();
                new ContentDAO().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDAO();
            var content = dao.GetByID(id); //lấy giá trị id truyền vào Content
            SetViewBag(content.CategoryID);
            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]//Thuộc tính ValidateInput (false) được sử dụng để cho phép gửi nội dung hoặc mã HTML đến máy chủ (dễ bị tấn công)
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContentDAO();
                var result = dao.Edit(model); //gán biến id vào DAO
                if (result > 0) //nếu insert thành công thì id>0
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "Content");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhập thất bại");
                }
            }
            SetViewBag(model.CategoryID);
            return View();
        }

        public void SetViewBag(long? selectedID = null)
        {
            var dao = new CategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.listAll(), "ID", "Name", selectedID); //lấy ra danh sách đưa vào dropdown
        }
    }
}