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
        public IActionResult Index(int pagina = 1, int tamanhoPagina = 10)
        {
            var todos = produtoDAO.GetProdutos();

            int totalItens = todos.Count;
            var produtosPaginados = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = "";
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return View(produtosPaginados);
        }

        [HttpPost]
        public IActionResult Salvar([FromForm] Produto produto, IFormFile ImagemUpload)
        {
            if (ImagemUpload != null && ImagemUpload.Length > 0)
            {
                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(ImagemUpload.FileName);

                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens/produtos");

                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    ImagemUpload.CopyTo(stream);
                }

                produto.Imagem = $"imagens/produtos/{nomeArquivo}";
            }
            else
            {
                produto.Imagem = Request.Form["Imagem"];
            }

            string valorStr = Request.Form["ValorProduto"];
            valorStr = valorStr.Replace(".", "").Replace(",", ".");
            decimal valor = decimal.Parse(valorStr, System.Globalization.CultureInfo.InvariantCulture);
            produto.ValorProduto = valor;

            new ProdutoDAO().Salvar(produto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult BuscarPorId(int id)
        {
            var produto = produtoDAO.BuscarPorId(id);
            if (produto == null)
                return NotFound();

            return PartialView("_Form", produto);
        }

        [HttpGet]
        public IActionResult Pesquisar(string termo, int pagina = 1, int tamanhoPagina = 10)
        {
            var todos = string.IsNullOrEmpty(termo)
                ? produtoDAO.GetProdutos()
                : produtoDAO.BuscarProduto(termo);

            int totalItens = todos.Count;
            var produtosPaginados = todos
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToList();

            ViewBag.Termo = termo;
            ViewBag.PaginaAtual = pagina;
            ViewBag.TotalItens = totalItens;
            ViewBag.TamanhoPagina = tamanhoPagina;

            return PartialView("_Lista", produtosPaginados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir([FromBody] List<int> ids)
        {
            foreach (var id in ids)
            {
                produtoDAO.Excluir(id);
            }

            return Ok();
        }
    }
}

