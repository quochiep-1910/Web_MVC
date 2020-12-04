using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Common;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index()
        {
            return View();
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
            if(ModelState.IsValid)
            {
                //var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                //model.CreatedBy = session.UserName;
                //var culture = Session[CommonConstants.CurrentCulture];
                //model.Language = culture.ToString();
                //new ContentDAO().Create(model);
                //return RedirectToAction("Index");
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
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {

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