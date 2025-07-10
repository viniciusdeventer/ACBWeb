using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ACBWeb.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDAO produtoDAO = new ProdutoDAO();

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var produtos = produtoDAO.GetProdutos();
            return View(produtos);
        }

        [HttpPost]
        public IActionResult Index(string termo)
        {
            List<Produto> produtos = string.IsNullOrEmpty(termo)
                ? produtoDAO.GetProdutos()
                : produtoDAO.BuscarProduto(termo);

            return View(produtos);
        }

        [HttpPost]
        public IActionResult ExcluirSelecionados(int[] selecionados)
        {
            if (selecionados != null && selecionados.Any())
            {
                foreach (var id in selecionados)
                {
                    produtoDAO.Excluir(id);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

