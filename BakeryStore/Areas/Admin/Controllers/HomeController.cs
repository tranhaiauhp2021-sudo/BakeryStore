using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Areas.Admin.Controllers {
 [AdminOnly] public class HomeController:Controller {
  private readonly MyStoreContext db=new MyStoreContext();
  public ActionResult Index(){ViewBag.ProductCount=db.Products.Count();ViewBag.CategoryCount=db.Categories.Count();ViewBag.CustomerCount=db.Customers.Count();ViewBag.OrderCount=db.Orders.Count();ViewBag.Revenue=db.Orders.Select(o=>(decimal?)o.TotalAmount).Sum()??0;return View(db.Orders.Include(o=>o.Customer).OrderByDescending(o=>o.OrderID).Take(8).ToList());}
  protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }
}
