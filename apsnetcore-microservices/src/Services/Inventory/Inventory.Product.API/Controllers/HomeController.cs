using Microsoft.AspNetCore.Mvc;

namespace Inventory.Product.API.Controllers
{
    public class HomeController : ControllerBase
    {
        // Cần sử dụng app.MapDefaultControllerRoute(); để map Home Index vào Url.
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
