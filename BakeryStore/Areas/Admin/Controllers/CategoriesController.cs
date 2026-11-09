using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BakeryStore.Models;
namespace BakeryStore.Areas.Admin.Controllers {
 [AdminOnly] public class CategoriesController:Controller {
  readonly MyStoreContext db=new MyStoreContext();
  public ActionResult Index(string search){var q=db.Categories.AsQueryable();if(!string.IsNullOrWhiteSpace(search))q=q.Where(c=>c.CategoryName.Contains(search));return View(q.OrderBy(c=>c.CategoryID).ToList());}
  [HttpGet] public ActionResult Create(){return View(new Category());}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Create([Bind(Include="CategoryName")] Category c){if(ModelState.IsValid){db.Categories.Add(c);db.SaveChanges();TempData["Success"]="Đã thêm danh mục.";return RedirectToAction("Index");}return View(c);}
  [HttpGet] public ActionResult Edit(int id){var c=db.Categories.Find(id);if(c==null)return HttpNotFound();return View(c);}
  [HttpPost,ValidateAntiForgeryToken] public ActionResult Edit(int id,[Bind(Include="CategoryID,CategoryName")] Category c){if(id!=c.CategoryID)return HttpNotFound();if(ModelState.IsValid){var current=db.Categories.Find(id);if(current==null)return HttpNotFound();current.CategoryName=c.CategoryName;db.SaveChanges();TempData["Success"]="Đã cập nhật danh mục.";return RedirectToAction("Index");}return View(c);}
  [HttpGet] public ActionResult Delete(int id){var c=db.Categories.Find(id);if(c==null)return HttpNotFound();return View(c);}
  [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public ActionResult DeleteConfirmed(int id){var c=db.Categories.Find(id);if(c==null)return HttpNotFound();if(db.Products.Any(p=>p.CategoryID==id)){TempData["Error"]="Danh mục đang có sản phẩm, không thể xóa.";return RedirectToAction("Index");}db.Categories.Remove(c);db.SaveChanges();return RedirectToAction("Index");}
  protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }
}
