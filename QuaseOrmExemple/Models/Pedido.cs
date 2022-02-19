using System.ComponentModel.DataAnnotations;

namespace QuaseOrmExemple.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        public int Numero { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime Data { get; set; }
        public bool Pago { get; set; }
        public List<Item> Itens { get; set; }
    }
}
