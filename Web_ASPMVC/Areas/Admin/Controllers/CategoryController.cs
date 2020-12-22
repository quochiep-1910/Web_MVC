using Models.DAO;
using Models.EF;
using System.Web.Mvc;
using Web_ASPMVC.Common;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            var dao = new CategoryDAO();
            var model = dao.listAll();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            var currentCulture = Session[CommonConstants.CurrentCulture]; //lấy dữ liệu từ CommonConstants.CurrentCulture
            model.Language = currentCulture.ToString(); //truyền vào language trong sql
            if (ModelState.IsValid)
            {
                var id = new CategoryDAO().Insert(model); //insert vào Category
                if (id > 0)
                {
                    //nếu có giá trị >0 trả về Index
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", StaticResource.Resource.InsertCategoryFailed); //thông báo lỗi qua resourse(cách này khá tiện )
                }
            }
            return View(model);
        }
    }
}