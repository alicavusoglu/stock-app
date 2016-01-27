using AutoMapper;
using StokApp.Models;
using StokApp.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace StokApp.Controllers
{
    public partial class UserController : Controller
    {
        StockAppDataContext _dataContext;

        StockAppDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new StockAppDataContext();
                return _dataContext;
            }
        }

        [HttpGet]
        public virtual ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public virtual ActionResult Login(LoginVM vm)
        {
            if (vm.Username == "kerim" && vm.Password == "1248")
            {
                FormsAuthentication.SetAuthCookie("kerim", vm.RememberMe);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}