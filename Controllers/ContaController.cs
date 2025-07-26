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

        [HttpPost]
        public IActionResult Salvar(Conta conta)
        {
            if (conta.Situacao == 0 && conta.IdConta > 0)
            {
                contaDAO.PagarConta(conta); 
            }
            else
            {
                contaDAO.Salvar(conta);
            }

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

        [HttpPost]
        public IActionResult Excluir(int id)
        {

            contaDAO.Excluir(id);
            return PartialView("_Form");
        }
    }
}

