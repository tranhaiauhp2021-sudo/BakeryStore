using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Areas.Admin.Controllers {
 [AdminOnly] public class ProductsController:Controller {
  readonly MyStoreContext db=new MyStoreContext();
  public ActionResult Index(string search,int? categoryId,string sort="new",int page=1){
   var q=db.Products.Include(p=>p.Category).AsQueryable();
   if(!string.IsNullOrWhiteSpace(search))q=q.Where(p=>p.ProductName.Contains(search));
   if(categoryId.HasValue)q=q.Where(p=>p.CategoryID==categoryId.Value);
   if(sort=="price_asc")q=q.OrderBy(p=>p.ProductPrice);else if(sort=="price_desc")q=q.OrderByDescending(p=>p.ProductPrice);else q=q.OrderByDescending(p=>p.ProductID);
   page=Math.Max(1,page);int size=10,total=q.Count();ViewBag.Search=search;ViewBag.CategoryId=categoryId;ViewBag.Sort=sort;ViewBag.Page=page;ViewBag.TotalPages=Math.Max(1,(total+size-1)/size);ViewBag.Categories=new SelectList(db.Categories.ToList(),"CategoryID","CategoryName",categoryId);return View(q.Skip((page-1)*size).Take(size).ToList());
  }
  void LoadCategories(object selected=null){ViewBag.Categories=new SelectList(db.Categories.OrderBy(c=>c.CategoryName).ToList(),"CategoryID","CategoryName",selected);}
  string SavePhoto(HttpPostedFileBase file){
   if(file==null||file.ContentLength==0)return null;
   string ext=Path.GetExtension(file.FileName).ToLowerInvariant();
   if(file.ContentLength>3*1024*1024||!(ext==".png"||ext==".jpg"||ext==".jpeg"||ext==".webp")||!new[]{"image/jpeg","image/png","image/webp"}.Contains(file.ContentType.ToLowerInvariant())){ModelState.AddModelError("","Chỉ hỗ trợ ảnh JPG/PNG/WEBP tối đa 3 MB.");return null;}
   string folder=Server.MapPath("~/Content/uploads");Directory.CreateDirectory(folder);
   string name=Guid.NewGuid().ToString("N")+ext;file.SaveAs(Path.Combine(folder,name));return "/Content/uploads/"+name;
  }
  [HttpGet] public ActionResult Create(){LoadCategories();return View(new Product());}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Create([Bind(Include="CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage")] Product p,HttpPostedFileBase photo){
   string image=SavePhoto(photo);if(image!=null)p.ProductImage=image;
   if(p.ProductPrice<=0)ModelState.AddModelError("ProductPrice","Giá phải lớn hơn 0.");
   if(ModelState.IsValid){db.Products.Add(p);db.SaveChanges();TempData["Success"]="Đã thêm sản phẩm.";return RedirectToAction("Index");}LoadCategories(p.CategoryID);return View(p);
  }
  [HttpGet] public ActionResult Edit(int id){var p=db.Products.Find(id);if(p==null)return HttpNotFound();LoadCategories(p.CategoryID);return View(p);}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Edit(int id,[Bind(Include="ProductID,CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage")] Product p,HttpPostedFileBase photo){
   if(p.ProductID!=id)return HttpNotFound();string image=SavePhoto(photo);if(image!=null)p.ProductImage=image;
   if(p.ProductPrice<=0)ModelState.AddModelError("ProductPrice","Giá phải lớn hơn 0.");
   if(ModelState.IsValid){var current=db.Products.Find(id);if(current==null)return HttpNotFound();current.CategoryID=p.CategoryID;current.ProductName=p.ProductName;current.ProductDecription=p.ProductDecription;current.ProductPrice=p.ProductPrice;current.ProductImage=p.ProductImage;db.SaveChanges();TempData["Success"]="Đã sửa sản phẩm.";return RedirectToAction("Index");}LoadCategories(p.CategoryID);return View(p);
  }
  [HttpGet] public ActionResult Delete(int id){var p=db.Products.Find(id);if(p==null)return HttpNotFound();return View(p);}
  [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public ActionResult DeleteConfirmed(int id){var p=db.Products.Find(id);if(p==null)return HttpNotFound();if(db.OrderDetails.Any(x=>x.ProductID==id)){TempData["Error"]="Sản phẩm đã có đơn hàng, không thể xóa.";return RedirectToAction("Index");}db.Products.Remove(p);db.SaveChanges();return RedirectToAction("Index");}
  protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }
}
