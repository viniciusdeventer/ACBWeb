using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class ContaController : Controller
    {
        private readonly ContaDAO contaDAO = new ContaDAO();

        [HttpGet]
        public IActionResult Index(int id)
        {
            var contas = contaDAO.GetContas(id);
            return View(contas);
        }

        [HttpPost]
        public IActionResult Salvar(Conta conta)
        {
            if (conta.IdConta == null)
            {
                conta.DataAbertura = DateTime.Now;
                conta.Situacao ??= 0;
            }

            contaDAO.Salvar(conta);
            return RedirectToAction("BuscarPorId", new { id = conta.IdCliente });
        }

        [HttpGet]
        public IActionResult BuscarContaPorId(int id)
        {
            var conta = contaDAO.BuscarPorId(id);
            return PartialView("_Form", conta);
        }
    }
}

