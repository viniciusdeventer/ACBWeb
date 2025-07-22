namespace ACBWeb.ViewModels
{
    public class CaixaMensalVM
    {
        public DateTime Dia { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public int TotalItens { get; set; }
        public decimal MaiorValor { get; set; }
        public decimal MediaDiaria { get; set; }
    }
}
