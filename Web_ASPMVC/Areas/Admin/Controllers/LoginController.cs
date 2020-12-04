﻿using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_ASPMVC.Areas.Admin.Models;
using Web_ASPMVC.Common;

namespace Web_ASPMVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)//kiểm tra rỗng
            {
                var dao = new UserDAO();
                var result = dao.Login(model.UserName, model.Password);
                if(result == 1)
                {
                    var user = dao.GetByID(model.UserName); //lấy ra được UserName
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;  //ta gán UserName vào userSession
                    userSession.UserID = user.ID;          // ta gán UID vào userSession
                    Session.Add(CommonConstants.USER_SESSION,userSession); //Gán USER_SESSION vào userSession 
                    return RedirectToAction("Index", "Home");// trả về Index
                } else if(result==0)
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
            }
            return View("Index");
        }
    }
}