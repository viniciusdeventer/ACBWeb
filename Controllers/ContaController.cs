using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class ContaController : Controller
    {
        private readonly ContaDAO contaDAO = new ContaDAO();
        private readonly ContaProdutosDAO contaProdutosDAO = new ContaProdutosDAO();

        //[HttpGet]
        //public IActionResult Index(int id)
        //{
        //    var contas = contaDAO.GetContas(id);
        //    return View(contas);
        //}

        [HttpPost]
        public IActionResult Salvar(Conta conta)
        {
            if (conta.IdConta == null)
            {
                conta.DataCadastro = DateTime.Now;
                conta.Situacao ??= 0;
            }

            contaDAO.Salvar(conta);
            return PartialView("_Form", conta);
        }

        [HttpGet]
        public IActionResult BuscarContaPorId(int id, int pagina = 1, int tamanhoPagina = 10)
        {
            var conta = contaDAO.BuscarPorId(id);

            var contaProdutos = contaProdutosDAO.GetContaProdutos(id);

            int totalItens = contaProdutos.Count;
            var contaProdutosPaginados = contaProdutos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            ViewBag.ContaProdutos = contaProdutosPaginados;

            return PartialView("_Form", conta);
        }
    }
}

