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
        public IActionResult Index(DateTime? data, int pagina = 1, int tamanhoPagina = 10)
        {
            var dataFiltro = data ?? DateTime.Today;

            var todos = vendaDAO.GetVendas(dataFiltro);

            int totalItens = todos.Count;
            var vendasPaginadas = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.DataSelecionada = dataFiltro;
            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return View(vendasPaginadas);
        }

        [HttpGet]
        public IActionResult BuscarPorId(int id)
        {
            var venda = vendaDAO.BuscarPorId(id);
            if (venda == null)
                return NotFound();

            return PartialView("_Form", venda);
        }

        [HttpPost]
        public IActionResult Salvar(Venda venda)
        {
            vendaDAO.Salvar(venda);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {

            vendaDAO.Excluir(id);
            return RedirectToAction("Index");
        }
    }
}
