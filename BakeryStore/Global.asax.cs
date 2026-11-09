using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using BakeryStore.Models;
namespace BakeryStore {
 public class MvcApplication : System.Web.HttpApplication {
  protected void Application_Start() { Database.SetInitializer<MyStoreContext>(null); AreaRegistration.RegisterAllAreas(); RouteConfig.RegisterRoutes(RouteTable.Routes); }
 }
}
