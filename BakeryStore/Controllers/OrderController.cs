using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Controllers {
 public class OrderController:Controller {
  private readonly MyStoreContext db=new MyStoreContext();
  private Customer CurrentCustomer(){var username=Session["Username"] as string;return username==null?null:db.Customers.FirstOrDefault(x=>x.Username==username);}
  [HttpGet] public ActionResult Checkout(){
   var customer=CurrentCustomer();if(customer==null)return RedirectToAction("Login","Account",new{returnUrl=Url.Action("Checkout","Order")});
   var cart=CartController.GetCart(Session);if(!cart.Any())return RedirectToAction("Index","Cart");
   return View(new CheckoutVM{AddressDelivery=customer.CustomerAddress,Cart=new CartVM{Items=cart}});
  }
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Checkout(CheckoutVM vm){
   var customer=CurrentCustomer();if(customer==null)return RedirectToAction("Login","Account");
   var cart=CartController.GetCart(Session);if(cart.Count==0)return RedirectToAction("Index","Cart");
   if(string.IsNullOrWhiteSpace(vm.AddressDelivery)){ModelState.AddModelError("AddressDelivery","Vui lòng nhập địa chỉ nhận hàng.");vm.Cart=new CartVM{Items=cart};return View(vm);}
   using(var tx=db.Database.BeginTransaction()){
    // Giá luôn được lấy lại từ database; không tin giá ở client / Session.
    var ids=cart.Select(c=>c.ProductID).ToList();var products=db.Products.Where(p=>ids.Contains(p.ProductID)).ToList();
    if(products.Count!=ids.Count){ModelState.AddModelError("","Có sản phẩm không còn tồn tại. Hãy xóa và thêm lại giỏ hàng.");vm.Cart=new CartVM{Items=cart};return View(vm);}
    var order=new Order{CustomerID=customer.CustomerID,OrderDate=DateTime.Today,PaymentStatus="Chờ xác nhận - COD",AddressDelivery=vm.AddressDelivery.Trim()};
    foreach(var item in cart){var p=products.First(x=>x.ProductID==item.ProductID);order.OrderDetails.Add(new OrderDetail{ProductID=p.ProductID,Quantity=Math.Min(99,Math.Max(1,item.Quantity)),UnitPrice=p.ProductPrice});}
    order.TotalAmount=order.OrderDetails.Sum(i=>i.Quantity*i.UnitPrice);db.Orders.Add(order);db.SaveChanges();tx.Commit();
    Session["Cart"]=null;return RedirectToAction("OrderSuccess",new{id=order.OrderID});
   }
  }
  public ActionResult OrderSuccess(int id){var customer=CurrentCustomer();if(customer==null)return RedirectToAction("Login","Account");var order=db.Orders.Include(x=>x.OrderDetails.Select(d=>d.Product)).FirstOrDefault(x=>x.OrderID==id&&x.CustomerID==customer.CustomerID);if(order==null)return HttpNotFound();return View(order);}
  public ActionResult OrderHistory(){var customer=CurrentCustomer();if(customer==null)return RedirectToAction("Login","Account");return View(db.Orders.Include(x=>x.OrderDetails).Where(x=>x.CustomerID==customer.CustomerID).OrderByDescending(x=>x.OrderID).ToList());}
  protected override void Dispose(bool disposing){if(disposing)db.Dispose();base.Dispose(disposing);}
 }
}
