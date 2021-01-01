using Models.DAO;
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
        public ActionResult Index()
        {
            var dao = new ProductCategoryDAO();
            var model = dao.ListAll();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}