using System;

namespace QuaseOrm.DataAnnotations
{
    public class TableAttribute : Attribute
    {
        public TableAttribute(string nome)
        {
            this.Nome = nome;
        }

        public string Nome { get; set; }
    }
}