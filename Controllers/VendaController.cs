using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class VendaController : Controller
    {
        private readonly VendaDAO vendaDAO = new VendaDAO();

        [Authorize]
        [HttpGet]
        public IActionResult Index(int pagina = 1, int tamanhoPagina = 10)
        {
            var todos = vendaDAO.GetVendas();

            int totalItens = todos.Count;
            var vendasPaginadas = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return View(vendasPaginadas);
        }
    }
}
