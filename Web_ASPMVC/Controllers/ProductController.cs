using Models.DAO;
using System;
using System.Web.Mvc;

namespace Web_ASPMVC.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// lấy ra giá trị cho thanh menu main truyền vào view
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]//chỉ được gọi thông qua Action, không gọi trực tiếp được
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDAO().ListAll();
            return PartialView(model);
        }

        /// <summary>
        /// Lấy thông tin của ProductCategory và Product truyền vào view
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        public ActionResult Category(long cateid, int page = 1, int pageSize = 4)
        {
            var category = new ProductCategoryDAO().ViewDetail(cateid);//lấy ra tất cả thông tin thông qua id truyền vào
            ViewBag.Category = category;
            int totalRecord = 0;
            var model = new ProductDAO().ListByCategoryId(cateid, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));  //lấy số bản ghi đến được trong ProductDAO / cho pageSize và ép kiểu sang double để làm tròn lên và chuyển lại kiểu int (tổng số trang hiện thị)
            ViewBag.TotalPage = totalPage;//truyền totalPage vào viewbag
            //key tạo nút next và prev
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1; //trang đầu
            ViewBag.Last = totalPage; //trang cuối cùng
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }

        /// <summary>
        /// lấy ra thông tin từ Product và ProductCategory truyền vào view
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        //cách 1:  //[OutputCache(Location =System.Web.UI.OutputCacheLocation.Server ,Duration =int.MaxValue,VaryByParam = "cateid")] đảm bảo mỗi lần load lên là id khác ko trùng,
        //load trong ram 1 lần, lần sau sẽ nhanh hơn ko cần gọi từ server nếu ko có sự thay đổi
        //cách 2
        [OutputCache(CacheProfile = "Cache1HourForProduct")]
        public ActionResult Detail(long cateid)
        {
            var product = new ProductDAO().ViewDetail(cateid); ////lấy ra tất cả thông tin thông qua id truyền vào
            ViewBag.Category = new ProductCategoryDAO().ViewDetail(product.CategoryID.Value); //lấy tất cả thuộc tính thông qua viewdetail truyền vô viewbag
            ViewBag.RelatedProducts = new ProductDAO().ListRelatedProduct(cateid); ///lấy tất cả thuộc tính thông qua viewdetail truyền vô viewbag

            return View(product);//chuyển thông tin cho view hiện th
        }

        /// <summary>
        /// lấy ra từ khoá mà truyền vào search
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public JsonResult ListName(string term)
        {
            var data = new ProductDAO().ListName(term);
            return Json(new
            {
                data = data,
                status = true
            },
            JsonRequestBehavior.AllowGet); //cho phép get
        }

        /// <summary>
        /// Tìm kiếm sản phẩm theo từ khoá nhập vào
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Search(string keyword, int page = 1, int pageSize = 4)
        {
            int totalRecord = 0;
            var model = new ProductDAO().Search(keyword, ref totalRecord, page, pageSize); //nên kiểm tra lại totalRecord xem nó đưa ra tổng bao nhiêu sản phẩm để phân trang
            ViewBag.keyword = keyword; //truyền từ khoá vào viewbag
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));  //lấy số bản ghi đến được trong ProductDAO / cho pageSize và ép kiểu sang double để làm tròn lên và chuyển lại kiểu int (tổng số trang hiện thị)
            ViewBag.TotalPage = totalPage;//truyền totalPage vào viewbag
            //key tạo nút next và prev
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1; //trang đầu
            ViewBag.Last = totalPage; //trang cuối cùng
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }
    }
}