using System;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Controllers {
 public class AccountController:Controller {
  private readonly MyStoreContext db=new MyStoreContext();
  [HttpGet] public ActionResult Register(){return View(new RegisterVM());}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Register(RegisterVM vm){
   if(db.Users.Any(x=>x.Username==vm.Username))ModelState.AddModelError("Username","Tên đăng nhập đã tồn tại.");
   if(!ModelState.IsValid)return View(vm);
   var user=new UserAccount{Username=vm.Username.Trim(),Password=PasswordTool.Hash(vm.Password),UserRole="C"};
   var customer=new Customer{CustomerName=vm.CustomerName,CustomerPhone=vm.CustomerPhone,CustomerEmail=vm.CustomerEmail,CustomerAddress=vm.CustomerAddress,Username=user.Username};
   db.Users.Add(user);db.Customers.Add(customer);db.SaveChanges();
   Session["Username"]=user.Username;Session["UserRole"]="C";return RedirectToAction("Index","Home");
  }
  [HttpGet] public ActionResult Login(string returnUrl){return View(new LoginVM{ReturnUrl=returnUrl});}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Login(LoginVM vm){
   if(!ModelState.IsValid)return View(vm);
   var user=db.Users.Find(vm.Username);
   if(user==null||!PasswordTool.Verify(vm.Password,user.Password)){ModelState.AddModelError("","Sai tên đăng nhập hoặc mật khẩu.");return View(vm);}
   Session["Username"]=user.Username;Session["UserRole"]=user.UserRole.Trim();
   if(!string.IsNullOrEmpty(vm.ReturnUrl)&&Url.IsLocalUrl(vm.ReturnUrl))return Redirect(vm.ReturnUrl);
   return (user.UserRole.Trim()=="A")?RedirectToAction("Index","Home",new{area="Admin"}):RedirectToAction("Index","Home",new{area=""});
  }
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Logout(){Session.Clear();Session.Abandon();return RedirectToAction("Index","Home");}
  [HttpGet] public ActionResult Edit(){var username=Session["Username"] as string;if(username==null)return RedirectToAction("Login");var item=db.Customers.FirstOrDefault(x=>x.Username==username);if(item==null)return RedirectToAction("Index","Home");return View(item);}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Edit(Customer input){
   var username=Session["Username"] as string;if(username==null)return RedirectToAction("Login");
   var customer=db.Customers.FirstOrDefault(x=>x.Username==username);if(customer==null)return HttpNotFound();
   if(string.IsNullOrWhiteSpace(input.CustomerName)||string.IsNullOrWhiteSpace(input.CustomerPhone)){ModelState.AddModelError("","Vui lòng nhập họ tên và số điện thoại.");return View(customer);}
   customer.CustomerName=input.CustomerName;customer.CustomerPhone=input.CustomerPhone;customer.CustomerEmail=input.CustomerEmail;customer.CustomerAddress=input.CustomerAddress;db.SaveChanges();TempData["Success"]="Đã cập nhật thông tin!";return RedirectToAction("Edit");
  }
  protected override void Dispose(bool disposing){if(disposing)db.Dispose();base.Dispose(disposing);}
 }
}
