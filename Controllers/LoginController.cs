using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ACBWeb.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string senha)
        {
            var login = LoginDAO.Login(usuario, senha);

            if (login != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.Nome)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Venda");
            }

            ViewBag.Erro = "Usuário ou senha inválidos.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
