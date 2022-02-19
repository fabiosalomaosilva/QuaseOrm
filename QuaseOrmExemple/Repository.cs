using QuaseOrm;
using QuaseOrmExemple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaseOrmExemple
{
    public class Repository
    {
        private static string connectionString = @"Data Source=localhost\;Initial Catalog=TesteOrm;Persist Security Info=True;User ID=sa;Password=teste.123";

        public static Pedido AddPedido(Pedido pedido)
        {
            using var db = new Session(connectionString);
            var p = db.Add(pedido);
            foreach (var i in pedido.Itens)
            {
                i.PedidoId = p.Id;
                db.Add(i);
            }

            return p;
        }

        public static List<Pedido> GetAll()
        {
            using var db = new Session(connectionString);
            return db.GetAll<Pedido>();
        }

    }
}
