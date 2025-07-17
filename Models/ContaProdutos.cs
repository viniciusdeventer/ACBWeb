namespace ACBWeb.Models
{
    public class ContaProdutos
    {
        public int? IdItem { get; set; }
        public String? NomeItem { get; set; }
        public int? IdConta { get; set; }
        public int? IdProduto { get; set; }
        public int? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public int? TipoItem { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
