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
        public IActionResult DetalhesContas(int idCliente)
        {
            var contas = contaDAO.GetContas(idCliente);
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
        public IActionResult BuscarContaPorId(int idConta)
        {
            var conta = contaDAO.BuscarPorId(idConta);
            if (conta == null) return NotFound();

            return PartialView("_Form", conta);
        }
    }
}

