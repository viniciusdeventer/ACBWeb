namespace ACBWeb.Models
{
    public class Produto
    {
        public int? IdProduto { get; set; }
        public String? Nome { get; set; }
        public String? Descricao { get; set; }
        public String? Imagem { get; set; }
        public decimal? ValorProduto { get; set; }
        public int? Estoque { get; set; }
        public int? Status { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
