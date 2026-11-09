using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Areas.Admin.Controllers {
 [AdminOnly] public class CustomersController:Controller {
  readonly MyStoreContext db=new MyStoreContext();
  public ActionResult Index(string search){var q=db.Customers.AsQueryable();if(!string.IsNullOrWhiteSpace(search))q=q.Where(x=>x.CustomerName.Contains(search)||x.Username.Contains(search)||x.CustomerPhone.Contains(search));ViewBag.Search=search;return View(q.OrderByDescending(x=>x.CustomerID).ToList());}
  public ActionResult Details(int id){var customer=db.Customers.Include(x=>x.Orders.Select(o=>o.OrderDetails)).FirstOrDefault(x=>x.CustomerID==id);if(customer==null)return HttpNotFound();return View(customer);}
  protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }
}
