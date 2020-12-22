using Models.DAO;
using Models.EF;
using System;
using System.Web.Mvc;

namespace Web_ASPMVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDAO().GetContact();
            return View(model);
        }

        [HttpPost]
        public JsonResult Send(string name, string email, string phone, string address, string message)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Email = email;
            feedback.CreatedDate = DateTime.Now;
            feedback.Phone = phone;
            feedback.Address = address;
            feedback.Content = message;

            var id = new ContactDAO().Insert(feedback);
            if (id > 0) //nếu có giá trị truyền vào
            {
                return Json(new
                {
                    status = true
                });
                //send mail
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}