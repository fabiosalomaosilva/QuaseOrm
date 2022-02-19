using System;

namespace QuaseOrm.DataAnnotations
{
    /// <summary>
    /// Atributo para verificação da chave estrangeira
    /// </summary>
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string nome)
        {
            this.Nome = nome;
        }

        public string Nome { get; set; }
    }
}