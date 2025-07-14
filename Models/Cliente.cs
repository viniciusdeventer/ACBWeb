namespace ACBWeb.Models
{
    public class Cliente
    {
        public int? IdCliente { get; set; }
        public String? Nome { get; set; }
        public String? Telefone { get; set; }
        public String? Observacoes { get; set; }
        public int? Status { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
