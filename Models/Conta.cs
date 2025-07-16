namespace ACBWeb.Models
{
    public class Conta
    {
        public int? IdConta { get; set; }
        public int? IdCliente { get; set; }

        public String? NomeCliente { get; set; }
        public int? Situacao { get; set; }
        public decimal? ValorPagamento { get; set; }
        public String? ObservacaoPagamento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
