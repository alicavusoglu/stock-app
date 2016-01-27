using AutoMapper;
using StokApp.Models;
using StokApp.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StokApp.Controllers
{
    [Authorize]
    public partial class HomeController : Controller
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

        public virtual ActionResult Index()
        {

            return View();
        }



        [ActionName("hareketler")]
        public ActionResult Transactions()
        {
            ViewBag.PageName = "transactions";
            return View("Transactions");
        }

        [ActionName("yeni-hareket")]
        [HttpGet]
        public virtual ActionResult NewTransaction()
        {
            ViewBag.Products = DataContext.Products.Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            }).ToList()
            .Select(s => new Product
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return View("Transaction", new StockTransactionEditVM()
            {
                Date = DateTime.Now,
                TransactionType = StockTransactionType.Output
            });
        }

        [ActionName("yeni-hareket")]
        [HttpPost]
        public virtual ActionResult NewTransaction(StockTransactionEditVM vm)
        {
            ViewBag.Products = DataContext.Products.Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            }).ToList()
            .Select(s => new Product
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            if (ModelState.IsValid)
            {
                DataContext.StockTransactions.InsertOnSubmit(Mapper.Map<StockTransaction>(vm));
                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Stok Hareketi Eklendi";
                return RedirectToAction("hareketler");
            }

            return View("Transaction",vm);
        }

        [ActionName("hareket-duzenle")]
        [HttpGet]
        public virtual ActionResult EditTransaction(int id)
        {
            ViewBag.Products = DataContext.Products.Select(s => new
            {
                Id = s.Id,
                Name = s.Name
            }).ToList()
            .Select(s => new Product
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            var model = Mapper.Map<StockTransactionEditVM>(DataContext.StockTransactions.Where(w => w.Id == id).First());
            return View("Transaction", model);
        }

        [ActionName("hareket-duzenle")]
        [HttpPost]
        public virtual ActionResult EditTransaction(StockTransactionEditVM vm)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<StockTransaction>(vm);
                var data = DataContext.StockTransactions.Where(w => w.Id == model.Id).First();
                data.Date = model.Date;
                data.Description = model.Description;
                data.Input = model.Input;
                data.Output = model.Output;
                data.ProductRef = model.ProductRef;

                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Hareket Güncellendi";
                return RedirectToAction("hareketler");
            }
            else {
                ViewBag.Products = DataContext.Products.Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
                .Select(s => new Product
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();
                return View("Transaction", vm);
            }
        }

        public JsonResult GetTransactions(FormCollection param)
        {
            //param["search[value]"]
            var data = DataContext.StockTransactions.OrderByDescending(o => o.Date).ToList().Select(s => new StockTransactionListVM()
            {
                Amount = s.Input != 0 ? (s.Input + " Giriş") : (s.Output + " Çıkış"),
                Date = s.Date.ToShortDateString(),
                Description = s.Description ?? "",
                Id = s.Id,
                ProductText = s.Product.Name
            }).ToList();
            //var total = DataContext.StockTransactions.Count();

            //var data = DataContext.StockTransactions.Skip(int.Parse(param["start"])).Take(int.Parse(param["length"])).ToList();
            //var total = DataContext.StockTransactions.Count();

            int total = 0;
            return Json(new
            {
                draw = param["draw"],
                recordsTotal = total,
                recordsFiltered = total,
                data = data
            },
          JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteTransaction(int id)
        {
            var data = DataContext.StockTransactions.Where(w => w.Id == id).First();
            DataContext.StockTransactions.DeleteOnSubmit(data);
            DataContext.SubmitChanges();
            return Json(new
            {
                Result = true,
                Id = id
            }, JsonRequestBehavior.AllowGet);
        }




        [ActionName("urunler")]
        public ActionResult Products()
        {
            return View("Products");
        }

        [ActionName("yeni-urun")]
        [HttpGet]
        public virtual ActionResult NewProduct()
        {
            ViewBag.StockTypes = DataContext.StockTypes.Select(s => new
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList()
            .Select(s => new StockType
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList();

            return View("Product", new Product());
        }

        [ActionName("yeni-urun")]
        [HttpPost]
        public virtual ActionResult NewProduct(Product model)
        {
            ViewBag.StockTypes = DataContext.StockTypes.Select(s => new
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList()
            .Select(s => new StockType
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList();

            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                DataContext.Products.InsertOnSubmit(model);
                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Ürün Eklendi";
                return RedirectToAction("urunler");
            }

            return View("Product", model);
        }

        [ActionName("urun-duzenle")]
        [HttpGet]
        public virtual ActionResult EditProduct(int id)
        {
            ViewBag.StockTypes = DataContext.StockTypes.Select(s => new
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList()
            .Select(s => new StockType
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList();

            var model = DataContext.Products.Where(w => w.Id == id).First();
            return View("Product", model);
        }

        [ActionName("urun-duzenle")]
        [HttpPost]
        public virtual ActionResult EditProduct(Product model)
        {
            ViewBag.StockTypes = DataContext.StockTypes.Select(s => new
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList()
            .Select(s => new StockType
            {
                Id = s.Id,
                TypeName = s.TypeName,
                PiecesInType = s.PiecesInType
            }).ToList();

            if (ModelState.IsValid)
            {
                var data = DataContext.Products.Where(w => w.Id == model.Id).First();
                data.Name = model.Name;
                data.StockTypeRef = model.StockTypeRef;
                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Ürün Güncellendi";
                return RedirectToAction("urunler");
            }

            return View("Product", model);
        }

        public JsonResult GetProducts(FormCollection param)
        {
            //param["search[value]"]
            var data = DataContext.Products.OrderBy(o => o.Name).ToList().Select(s => new ProductListVM()
            {
                Id = s.Id,
                CreatedDate = s.CreatedDate,
                Name = s.Name,
                StockTypeText = s.StockType.TypeName,
                Amount = s.StockTransactions.Sum(ss => ss.Input) - s.StockTransactions.Sum(ss => ss.Output)
            }).ToList();
            //var total = DataContext.StockTypes.Count();

            //var data = DataContext.StockTransactions.Skip(int.Parse(param["start"])).Take(int.Parse(param["length"])).ToList();
            //var total = DataContext.StockTransactions.Count();
            int total = 0;

            return Json(new
            {
                draw = param["draw"],
                recordsTotal = total,
                recordsFiltered = total,
                data = data
            },
          JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProduct(int id)
        {
            var data = DataContext.Products.Where(w => w.Id == id).First();
            DataContext.Products.DeleteOnSubmit(data);
            DataContext.SubmitChanges();
            return Json(new
            {
                Result = true,
                Id = id
            }, JsonRequestBehavior.AllowGet);
        }





        [ActionName("birimler")]
        public ActionResult StockTypes()
        {
            return View("StockTypes");
        }

        [ActionName("yeni-birim")]
        [HttpGet]
        public virtual ActionResult NewStockType()
        {
            return View("StockType", new StockType() { PiecesInType = 1 });
        }

        [ActionName("yeni-birim")]
        [HttpPost]
        public virtual ActionResult NewStockType(StockType model)
        {
            if (ModelState.IsValid)
            {
                DataContext.StockTypes.InsertOnSubmit(model);
                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Stok Tipi Eklendi";
                return RedirectToAction("birimler");
            }

            return View("StockType", model);
        }

        [ActionName("birim-duzenle")]
        [HttpGet]
        public virtual ActionResult EditStockType(int id)
        {
            return View("StockType", DataContext.StockTypes.Where(w => w.Id == id).First());
        }

        [ActionName("birim-duzenle")]
        [HttpPost]
        public virtual ActionResult EditStockType(StockType model)
        {
            if (ModelState.IsValid)
            {
                var data = DataContext.StockTypes.Where(w => w.Id == model.Id).First();

                data.TypeName = model.TypeName;
                data.PiecesInType = model.PiecesInType;

                DataContext.SubmitChanges();

                TempData["ResultMessage"] = "Stok Tipi Güncellendi";
                return RedirectToAction("birimler");
            }

            return View("StockType", model);
        }

        public JsonResult GetStockTypes(FormCollection param)
        {
            //param["search[value]"]
            var data = DataContext.StockTypes.OrderBy(o => o.TypeName).ToList().Select(s => new StockTypeVM()
            {
                Id = s.Id,
                PiecesInType = s.PiecesInType,
                TypeName = s.TypeName
            }).ToList();
            //var total = DataContext.StockTypes.Count();

            //var data = DataContext.StockTransactions.Skip(int.Parse(param["start"])).Take(int.Parse(param["length"])).ToList();
            //var total = DataContext.StockTransactions.Count();
            int total = 0;

            return Json(new
            {
                draw = param["draw"],
                recordsTotal = total,
                recordsFiltered = total,
                data = data
            },
          JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteStockType(int id)
        {
            var data = DataContext.StockTypes.Where(w => w.Id == id).First();
            DataContext.StockTypes.DeleteOnSubmit(data);
            DataContext.SubmitChanges();
            return Json(new
            {
                Result = true,
                Id = id
            }, JsonRequestBehavior.AllowGet);
        }
    }
}