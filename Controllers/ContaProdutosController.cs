using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class ContaProdutosController : Controller
    {
        private readonly ContaProdutosDAO contaProdutosDAO = new ContaProdutosDAO();
        private readonly ProdutoDAO produtoDAO = new ProdutoDAO();

        [HttpGet]
        public IActionResult BuscarContaProdutosPorId(int id)
        {
            var contaProduto = contaProdutosDAO.BuscarPorId(id);
            if (contaProduto != null && contaProduto.IdProduto.HasValue)
            {
                var produto = produtoDAO.BuscarPorId(contaProduto.IdProduto.Value);
                if (produto != null)
                {
                    contaProduto.NomeItem = produto.Nome;
                }
            }
            return PartialView("_Form", contaProduto);
        }

        [HttpPost]
        public IActionResult Salvar(ContaProdutos contaProdutos)
        {
            string valorStr = Request.Form["ValorUnitario"];
            valorStr = valorStr.Replace(".", "").Replace(",", ".");
            decimal valor = decimal.Parse(valorStr, System.Globalization.CultureInfo.InvariantCulture);
            contaProdutos.ValorUnitario = valor;

            contaProdutosDAO.Salvar(contaProdutos);
            return PartialView("_Form", contaProdutos);
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {

            contaProdutosDAO.Excluir(id);
            return PartialView("_Form");
        }
    }
}
