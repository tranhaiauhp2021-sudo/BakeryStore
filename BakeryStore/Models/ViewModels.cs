using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BakeryStore.Models {
 public class HomeVM {public List<Product> Featured {get;set;}=new List<Product>(); public List<Product> Products {get;set;}=new List<Product>(); public List<Category> Categories {get;set;}=new List<Category>(); public string Search {get;set;} public int Page {get;set;} public int TotalPages {get;set;} }
 public class CatalogVM {public List<Product> Products {get;set;}=new List<Product>(); public List<Category> Categories {get;set;}=new List<Category>(); public int? CategoryId {get;set;} public string CategoryName {get;set;} public string Search {get;set;} public string Sort {get;set;} public int Page {get;set;} public int TotalPages {get;set;} }
 public class ProductDetailVM {public Product Product {get;set;} public List<Product> Related {get;set;}=new List<Product>(); public List<Product> Top {get;set;}=new List<Product>(); }
 public class CartItem {public int ProductID {get;set;} public string ProductName {get;set;} public string ProductImage {get;set;} public decimal ProductPrice {get;set;} public int Quantity {get;set;} public decimal Subtotal {get {return ProductPrice*Quantity;} } }
 public class CartVM {public List<CartItem> Items {get;set;}=new List<CartItem>(); public decimal Total {get {return Items.Sum(x=>x.Subtotal);} } }
 public class RegisterVM {
  [Required, StringLength(255), Display(Name="Tên đăng nhập")] public string Username {get;set;}
  [Required, StringLength(60,MinimumLength=8), DataType(DataType.Password), Display(Name="Mật khẩu")] public string Password {get;set;}
  [Required, Compare("Password"), DataType(DataType.Password), Display(Name="Nhập lại mật khẩu")] public string ConfirmPassword {get;set;}
  [Required, Display(Name="Họ và tên")] public string CustomerName {get;set;}
  [Required, StringLength(15), Display(Name="Số điện thoại")] public string CustomerPhone {get;set;}
  [EmailAddress, Display(Name="Email")] public string CustomerEmail {get;set;}
  [Display(Name="Địa chỉ")] public string CustomerAddress {get;set;}
 }
 public class LoginVM { [Required, Display(Name="Tên đăng nhập")] public string Username {get;set;} [Required, DataType(DataType.Password),Display(Name="Mật khẩu")] public string Password {get;set;} public string ReturnUrl {get;set;} }
 public class CheckoutVM { [Required,Display(Name="Địa chỉ nhận hàng")] public string AddressDelivery {get;set;} public CartVM Cart {get;set;} }
 public class AdminUserVM { [Required, StringLength(255)] public string Username {get;set;} [DataType(DataType.Password)] public string Password {get;set;} }
}
