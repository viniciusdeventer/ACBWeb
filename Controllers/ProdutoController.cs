using ACBWeb.DAL.DAO;
using ACBWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

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
                var nomeArquivo = Guid.NewGuid().ToString().ToUpper() + Path.GetExtension(ImagemUpload.FileName);

                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens/produtos");

                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = ImagemUpload.OpenReadStream())
                using (var image = SixLabors.ImageSharp.Image.Load(stream))
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(600, 0), 
                        Mode = ResizeMode.Max
                    }));

                    var encoder = new SixLabors.ImageSharp.Formats.Png.PngEncoder
                    {
                        CompressionLevel = PngCompressionLevel.Level8, 
                    };

                    using (var outputStream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        image.SaveAsPng(outputStream, encoder);
                    }
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

        [HttpGet]
        public IActionResult BuscarProdutosJson(string term)
        {
            var produtos = string.IsNullOrEmpty(term)
                ? produtoDAO.GetProdutos()
                : produtoDAO.BuscarProduto(term);

            var resultado = produtos.Select(p => new {
                id = p.IdProduto,
                text = p.Nome
            });

            return Json(resultado);
        }

        [HttpGet]
        public IActionResult BuscarProdutoJson(int id)
        {
            var produto = produtoDAO.BuscarPorId(id);
            if (produto == null)
                return Json(new { sucesso = false });

            return Json(new
            {
                sucesso = true,
                valorProduto = produto.ValorProduto
            });
        }
    }
}

