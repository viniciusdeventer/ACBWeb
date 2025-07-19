namespace ACBWeb.Models
{
    public class Venda
    {
        public int? IdVenda { get; set; }
        public int? IdItem { get; set; }
        public string? NomeItem { get; set; }
        public DateTime? DataVenda { get; set; }
        public int? Quantidade { get; set; }
        public decimal? ValorVenda { get; set; }
        public int? TipoVenda { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
