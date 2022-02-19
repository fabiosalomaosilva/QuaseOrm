using QuaseOrmExemple;
using QuaseOrmExemple.Models;


var r = new Random();

var item1 = new Item { Id = 1, Nome = "Camisa", Quantidade = 1, Valor = 2 };
var item2 = new Item { Id = 2, Nome = "Calça", Quantidade = 1, Valor = 3 };
var item3 = new Item { Id = 3, Nome = "Tenis", Quantidade = 1, Valor = 5 };


var pedido = new Pedido
{
    Numero = r.Next(10),
    Data = DateTime.Now,
    Pago = true,
    ValorTotal = 10,
    Itens = new List<Item> { item1, item2, item3 }
};

Console.WriteLine("Insirido informações no banco de dados");
Console.WriteLine("................");
var p = Repository.AddPedido(pedido);
Console.WriteLine("Dados inseridos com sucesso");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Buscando dados.....");



var lista = Repository.GetAll();

foreach (var i in lista)
{
    Console.WriteLine("Pedido: " + i.Numero + " - Valor: " + i.ValorTotal);
}
Console.WriteLine("Busca encerrada com sucesso!!");
