using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteDAO clienteDAO = new ClienteDAO();
        private readonly ContaDAO contaDAO = new ContaDAO();

        [Authorize]
        [HttpGet]
        public IActionResult Index(int pagina = 1, int tamanhoPagina = 10)
        {
            var todos = clienteDAO.GetClientes();

            int totalItens = todos.Count;
            var clientesPaginados = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return View(clientesPaginados);
        }

        [HttpPost]
        public IActionResult Salvar(Cliente cliente)
        {
            string telefoneStr = Request.Form["Telefone"];
            telefoneStr = new string(telefoneStr.Where(char.IsDigit).ToArray()); 
            cliente.Telefone = telefoneStr;

            new ClienteDAO().Salvar(cliente);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult BuscarPorId(int id, int pagina = 1, int tamanhoPagina = 10)
        {
            ViewBag.IdCliente = id;

            var cliente = clienteDAO.BuscarPorId(id);
            if (cliente == null)
                return NotFound();

            var contas = contaDAO.GetContas(id);

            int totalItens = contas.Count;
            var contasPaginadas = contas
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            ViewBag.Contas = contasPaginadas;

            return PartialView("_Form", cliente);
        }

        [HttpGet]
        public IActionResult Pesquisar(string termo, int pagina = 1, int tamanhoPagina = 10)
        {
            var todos = string.IsNullOrEmpty(termo)
                ? clienteDAO.GetClientes()
                : clienteDAO.BuscarCliente(termo);

            int totalItens = todos.Count;
            var clientesPaginados = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = termo;
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return PartialView("_Table", clientesPaginados);
        }
    }
}

