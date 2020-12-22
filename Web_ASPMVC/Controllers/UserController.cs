using BotDetect.Web.Mvc;
using Facebook;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using Web_ASPMVC.Common;
using Web_ASPMVC.Models;

namespace Web_ASPMVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null; //cho session = null để hệ thống kiểm tra
                                                          //lại xem tài này có session hay chưa. Nếu chưa thì phải login

            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var dao = new UserDAO();
            var result = dao.Login(model.UserName, model.Password);
            if (result == 1)
            {
                var user = dao.GetByID(model.UserName); //lấy ra được UserName
                var userSession = new UserLogin();
                userSession.UserName = user.UserName;  //ta gán UserName vào userSession
                userSession.UserID = user.ID;          // ta gán UID vào userSession
                Session.Add(CommonConstants.USER_SESSION, userSession); //Gán USER_SESSION vào userSession
                return RedirectToAction("Index", "Home");// trả về Index
            }
            else if (result == 0)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
            }
            else if (result == -1)
            {
                ModelState.AddModelError("", "Tài khoản đang bị khoá");
            }
            else if (result == -2)
            {
                ModelState.AddModelError("", "Mật Khẩu không đúng");
            }
            else
            {
                ModelState.AddModelError("", "Đăng nhập không đúng");
            }
            return View(model);
        }

        // Register: Đăng kí
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                if (dao.CheckUserName(model.UserName)) //nếu check user từ model truyền vào trùng với DAO thì xuất mã lỗi
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
                }
                else if (dao.CheckEmail(model.Email)) //nếu check Email từ model truyền vào trùng với DAO thì xuất mã lỗi
                {
                    ModelState.AddModelError("", "Email đã tồn tại!");
                }
                else //không có lỗi thì ta tạo tài khoản
                {
                    var user = new User(); //tạo đối tượng và truyền từ model vào EF
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = model.Password;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;

                    if (!string.IsNullOrEmpty(model.ProvinceID)) //model khác rỗng
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = int.Parse(model.DistrictID);
                    }
                    var result = dao.Insert(user); //insert user
                    if (result > 0) //nếu > 0 thì có tài khoản
                    {
                        ViewBag.Success = "Đăng kí thành công :) ";
                        model = new RegisterModel(); //reset toàn bộ model lại cho lần tạo tài khoản tiếp theo
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng kí không thành công :( ");
                        MvcCaptcha.ResetCaptcha("registerCapcha");
                    }
                }
            }
            return View(model);
        }

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url); //truyền vào url, port
                uriBuilder.Query = null;
                uriBuilder.Fragment = null; //tối ưu hoá cho màn hình
                uriBuilder.Path = Url.Action("FacebookCallback"); //gọi tới Action fbCallback
                return uriBuilder.Uri;
            }
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"], //truyền từ Web.config
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))//nếu token rỗng thì đăng nhập thành công và ngược lại
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new User();
                user.Email = email;
                user.UserName = email;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDAO().InsertForFacebook(user);
                if (resultInsert > 0) //nếu thêm thành công
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }

        /// <summary>
        /// load ra danh sách các tỉnh (droplist)
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadProvince()
        {
            //ko hiểu thì mở sang file /assets/Client/Data/Provinces_Data.xml đọc lướt qua sẽ hiểu
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/Client/Data/Provinces_Data.xml")); //load ra đối tượng trong assets
            var xElement = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province"); //lấy ra các item có attribute là type value là province(lọc)
            var list = new List<ProvinceModel>(); //khởi tạo 1 list theo định dạng ProvinceModel
            ProvinceModel province = null; //khởi tạo 1 biến có dạng ProvinceModel null
            foreach (var item in xElement)
            {
                province = new ProvinceModel(); //tạo ra 1 đối tượng
                //Attribute:đọc ra thuộc tính
                province.ID = int.Parse(item.Attribute("id").Value); //lấy ra id và value rồi parse sang int
                province.Name = item.Attribute("value").Value;
                list.Add(province); //add province theo định dạng list
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        /// <summary>
        /// load ra danh sách các quận huyện (droplist)
        /// </summary>
        /// <param name="provinceID"></param>
        /// <returns></returns>
        public JsonResult LoadDistrict(int provinceID)
        {
            //ko hiểu thì mở sang file /assets/Client/Data/Provinces_Data.xml đọc lướt qua sẽ hiểu
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/assets/Client/Data/Provinces_Data.xml")); //load ra đối tượng trong assets

            var xElement = xmlDoc.Element("Root").Elements("Item") //lấy phần tử có id bắt đầu bằng id của Province truyền vào
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID); //lọc theo type có values province và parse id từ Provinces_Data.xml trùng với id truyền vào

            var list = new List<DistrictModel>(); //khởi tạo 1 list theo định dạng ProvinceModel
            DistrictModel district = null; //khởi tạo 1 biến có dạng ProvinceModel null
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))//chạy lấy ra từng phần tử
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value); //lấy ra id và value rồi parse sang int
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district); //add province theo định dạng list
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
    }
}