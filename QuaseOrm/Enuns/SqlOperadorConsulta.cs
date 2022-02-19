using QuaseOrm.DataAnnotations;

namespace QuaseOrm.Enums
{
    public enum SqlOperadorConsulta
    {
        [SqlOperadorTexto(Operador = "=")]
        EQUALS,

        [SqlOperadorTexto(Operador = "LIKE")]
        LIKE,

        [SqlOperadorTexto(Operador = "NOT LIKE")]
        NOT_LIKE
    }
}