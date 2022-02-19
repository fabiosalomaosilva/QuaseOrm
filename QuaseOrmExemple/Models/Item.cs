using System.ComponentModel.DataAnnotations;

namespace QuaseOrmExemple.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public Pedido Pedido { get; set; }
        public int PedidoId { get; set; }
    }
}
