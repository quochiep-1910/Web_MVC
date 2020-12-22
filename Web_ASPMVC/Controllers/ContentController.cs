using Models.DAO;
using System;
using System.Web.Mvc;

namespace Web_ASPMVC.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = new ContentDAO().ListAllPaging(page, pageSize);
            int totalRecord = 0;
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

        public ActionResult Detail(long id)
        {
            var model = new ContentDAO().GetByID(id);
            ViewBag.Tags = new ContentDAO().ListTag(id); //truyền vào để lấy ra tag
            return View(model);
        }

        public ActionResult Tag(string tagId, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;

            var model = new ContentDAO().ListAllByTag(tagId, ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            ViewBag.Tag = new ContentDAO().GetTag(tagId);
            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(model);
        }
    }
}