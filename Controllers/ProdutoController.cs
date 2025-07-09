using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ACBWeb.Controllers
{
    [Authorize]
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}