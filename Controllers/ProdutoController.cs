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
        public IActionResult Teste(IFormCollection form, IFormFile ImagemUpload)
        {
            var debug = new
            {
                Nome = form["Nome"],
                Descricao = form["Descricao"],
                ValorProduto = form["ValorProduto"],
                Status = form["Status"],
                DataCadastro = form["DataCadastro"],
                TemImagem = ImagemUpload != null ? "SIM" : "NAO",
                NomeArquivo = ImagemUpload?.FileName
            };

            return Json(debug);
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

            Console.WriteLine($"Nome: {produto.Nome}, Descricao: {produto.Descricao}, Valor: {produto.ValorProduto}");

            new ProdutoDAO().Salvar(produto);
            return RedirectToAction("Index");
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

