using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Areas.Admin.Controllers {
 [AdminOnly] public class OrdersController:Controller {
  readonly MyStoreContext db=new MyStoreContext();
  public ActionResult Index(string search){var q=db.Orders.Include(o=>o.Customer).AsQueryable();if(!string.IsNullOrWhiteSpace(search))q=q.Where(x=>x.Customer.CustomerName.Contains(search)||x.Customer.Username.Contains(search));ViewBag.Search=search;return View(q.OrderByDescending(x=>x.OrderID).ToList());}
  public ActionResult Details(int id){var order=db.Orders.Include(o=>o.Customer).Include(o=>o.OrderDetails.Select(d=>d.Product)).FirstOrDefault(x=>x.OrderID==id);if(order==null)return HttpNotFound();return View(order);}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult UpdateStatus(int id,string status){var permitted=new[]{"Chờ xác nhận - COD","Đang chuẩn bị","Đang giao","Đã hoàn thành","Đã hủy"};if(!permitted.Contains(status))return new HttpStatusCodeResult(400);var o=db.Orders.Find(id);if(o==null)return HttpNotFound();o.PaymentStatus=status;db.SaveChanges();return RedirectToAction("Details",new{id});}
  protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }
}
