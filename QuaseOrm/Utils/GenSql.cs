using QuaseOrm.Enums;
using QuaseOrm.Helpers;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace QuaseOrm.Utils
{
    internal static class GenSql
    {
        internal static SqlDbType DefinirTipoParametro(Type t)
        {
            SqlParameter param = new SqlParameter();
            TypeConverter tc = TypeDescriptor.GetConverter(param.DbType);

            if (tc.CanConvertFrom(t))
            {
                param.DbType = (DbType)tc.ConvertFrom(t.Name);
            }
            else
            {
                string nome = t.Name;
                // tentar forçar a conversão
                try
                {
                    if (t.Name.Contains("Guid"))   
                    {
                        param.DbType = (DbType)tc.ConvertFrom("Guid");
                    }
                    else if (t.Name.Contains("Boolean"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("Boolean");
                    }
                    else if (t.Name.Contains("Bool"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("Bool");
                    }
                    else if (t.Name.Contains("DateTime"))
                    {
                        param.SqlDbType = SqlDbType.DateTime2;
                    }
                    else if (t.Name.Contains("string"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("string");
                    }
                    else if (t.Name.Contains("int"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("int");
                    }
                    else if (t.Name.Contains("long"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("long");
                    }
                    else if (t.Name.Contains("int64"))
                    {
                        param.DbType = (DbType)tc.ConvertFrom("int64");
                    }
                    else if (nome == "Byte[]")
                    {
                        param.SqlDbType = SqlDbType.VarBinary;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.Int;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Int16, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.SmallInt;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Int64, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.BigInt;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.DateTime2;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Guid, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.UniqueIdentifier;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.Bit;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Bool, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.Bit;
                    }
                    else if (t.FullName == "System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]")
                    {
                        param.SqlDbType = SqlDbType.Decimal;
                    }
                    else
                    {
                        param.DbType = (DbType)tc.ConvertFrom(t.Name);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return param.SqlDbType;
        }

        internal static string InsertSqlString<T>() where T : new()
        {
            Type tipo = typeof(T);
            var propriedades = Helper.DefinirPropriedadesBasicasInserir<T>();

            int tam = propriedades.Length;
            StringBuilder s = new StringBuilder();
            s.Append(" INSERT INTO  ");
            s.Append(tipo.Name);
            s.Append(" (");

            for (int i = 0; i < tam; i++)
            {
                if (propriedades[i].PropertyType.FullName.Substring(0, 6) == "System")
                {
                    if (i < (tam - 1))
                    {
                        s.Append(String.Format(" {0}, ", propriedades[i].Name));
                    }
                    else
                    {
                        s.Append(String.Format(" {0} ", propriedades[i].Name));
                    }
                }
                else if (propriedades[i].PropertyType.BaseType.Name == "Enum")
                {
                    if (i < (tam - 1))
                    {
                        s.Append(String.Format(" {0}, ", propriedades[i].Name));
                    }
                    else
                    {
                        s.Append(String.Format(" {0} ", propriedades[i].Name));
                    }
                }
            }
    
            s.Append(" ) ");
            s.Append(" VALUES (");
            for (int i = 0; i < tam; i++)
            {
                if (propriedades[i].PropertyType.FullName.Substring(0, 6) == "System")
                {
                    if (i < (tam - 1))
                    {
                        s.Append(String.Format(" @{0}, ", propriedades[i].Name));
                    }
                    else
                    {
                        s.Append(String.Format(" @{0} ", propriedades[i].Name));
                    }
                }
                else if (propriedades[i].PropertyType.BaseType.Name == "Enum")
                {
                    if (i < (tam - 1))
                    {
                        s.Append(String.Format(" @{0}, ", propriedades[i].Name));
                    }
                    else
                    {
                        s.Append(String.Format(" @{0} ", propriedades[i].Name));
                    }
                }
            }

            s.Append(" ) SELECT * FROM " + tipo.Name + " WHERE " + Helper.RecuperarChavePrimaria<T>().Name + " = SCOPE_IDENTITY()");

            return s.ToString();
        }

        internal static string UpdateSqlString<T>(Parameters campos) where T : new()
        {
            Type tipo = typeof(T);
            var listaPropriedades = Helper.DefinirPropriedadesBasicas<T>();

            int tam = listaPropriedades.Length - 1;
            StringBuilder s = new StringBuilder();
            s.Append("UPDATE ");
            s.Append(tipo.Name);
            s.Append(" SET ");

            for (int i = 0; i < tam; i++)
            {
                if (listaPropriedades[i].PropertyType.FullName.Substring(0, 6) == "System")
                {
                    int index;
                    if (i < (tam - 1))
                    {
                        if (listaPropriedades[i].Name == campos.Criteria.Key)
                        {
                            s.Append("");
                        }
                        else
                        {
                            s.Append(String.Format(" {0} = @{0},", listaPropriedades[i].Name));
                            index = 1;
                        }
                    }
                    else
                    {
                        if (listaPropriedades[i].Name == campos.Criteria.Key)
                        {
                            s.Append("");
                        }
                        else
                        {
                            s.Append(String.Format(" {0} = @{0}", listaPropriedades[i].Name));
                            index = 1;
                        }
                    }
                }
                else if (listaPropriedades[i].PropertyType.BaseType.Name == "Enum")
                {
                    if (i < (tam - 1))
                    {
                        s.Append(String.Format(" {0} = @{0},", listaPropriedades[i].Name));
                    }
                    else
                    {
                        s.Append(String.Format(" {0} = @{0}", listaPropriedades[i].Name));
                    }
                }
            }
            s.Append(" WHERE ");
            s.Append(" (");
            s.Append(campos.Criteria.Key);
            s.Append(" = @");
            s.Append(campos.Criteria.Key);
            s.Append(")");

            return s.ToString();
        }

        internal static string DeleteSqlString<T>(Parameters campos) where T : new()
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" DELETE FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE ");
            s.Append(campos.Criteria.Key);
            s.Append(" = @");
            s.Append(campos.Criteria.Key);

            return s.ToString();
        }

        internal static string SelectAllSqlString<T>()
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT * FROM ");
            s.Append(tipo.Name);

            return s.ToString();
        }

        internal static string SelectAllSqlUmCriterioString<T>(Criteria criterio)
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT * FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE ");
            s.Append(criterio.Key);
            s.Append(" = @Criterio");
            return s.ToString();
        }

        /**
        internal static string SelectCriteriosComJoins_SqlString<T>(Parametros campos) where T : new()
        {
            Type tipo = typeof(T);
            int tamIncludes = campos.Joins.Count;
            int indexIncludes = 0;


            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");
            var chave = Helper.RecuperarChavePrimaria<T>();

            if (campos.Propriedades != null)
            {
                int tam = campos.Propriedades.Count;
                int index = 0;

                s.Append(String.Format("{0}.{1} as {0}_{1}", tipo.Name, chave.Name));
                s.Append(", ");

                foreach (var i in campos.Propriedades)
                {
                    s.Append(String.Format(" {0}.{1} as {0}_{1}, ", tipo.Name, i));
                }
            }
            else
            {
                var tipo1 = Helper.DefinirPropriedadesBasicas<T>();

                foreach (var i in tipo1)
                {
                    s.Append(String.Format(" {0}.{1} as {0}_{1}, ", tipo.Name, i.Name));
                }

            }

            foreach (var i in campos.Joins)
            {
                Type tipo1 = Helper.RecuperarClassePorNome<T>(i);

                var ii = tipo1.DeclaringType;
                var tt = ii.GetType();
                var props = Helper.DefinirPropriedadesBasicas(tipo1);
                int tam = props.Length;
                int index = 0;

                foreach (var item in props)
                {
                    if (tam == 1)
                    {
                        s.Append(String.Format(" {0}.{1} as {0}_{1} ", tipo1.Name, item.Name));
                        if (indexIncludes == 0)
                        {
                            s.Append(", ");
                            indexIncludes++;
                        }
                        else
                        {
                            if (indexIncludes == tamIncludes - 1)
                            {
                                s.Append("");
                            }
                            else
                            {
                                s.Append(", ");
                                indexIncludes++;
                            }
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            s.Append(String.Format(" {0}.{1} as {0}_{1}, ", tipo1.Name, item.Name));
                            index = 1;
                        }
                        else
                        {
                            if (index == tam - 1)
                            {
                                s.Append(String.Format(" {0}.{1} as {0}_{1} ", tipo1.Name, item.Name));
                                if (indexIncludes == 0)
                                {
                                    s.Append(", ");
                                    indexIncludes++;
                                }
                                else
                                {
                                    if (indexIncludes == tamIncludes - 1)
                                    {
                                        s.Append("");
                                    }
                                    else
                                    {
                                        s.Append(", ");
                                        indexIncludes++;
                                    }
                                }
                            }
                            else
                            {
                                s.Append(String.Format(" {0}.{1} as {0}_{1}, ", tipo1.Name, item.Name));
                                index = index + 1;
                            }
                        }
                    }

                }

            }

            s.Append(" FROM ");
            s.Append(tipo.Name);

            foreach (var i in campos.Joins)
            {
                s.Append(" INNER JOIN ");
                s.Append(i);
                s.Append(" ON ");
                s.Append(tipo.Name);
                s.Append(".");
                s.Append(chave.Name);
                s.Append(" = ");
                s.Append(i);
                s.Append(".");
                Type tipo1 = Helper.RecuperarClassePorNome<T>(i);
                s.Append(Helper.RecuperarChaveEstrangeira(tipo1).Name);
            }



            if (campos.Criterios != null)
            {
                int tam = campos.Criterios.Count;
                int index = 0;

                foreach (var i in campos.Criterios)
                {
                    if (tam == 1)
                    {
                        s.Append(String.Format(" WHERE {0} = @{1}", i.Chave, i.Chave));
                    }
                    else
                    {
                        if (index == 0)
                        {
                            s.Append(String.Format(" WHERE {0} = @{1} {2}", i.Chave, i.Chave, "AND"));
                            index = 1;
                        }
                        else
                        {
                            if (index == tam - 1)
                            {
                                s.Append(String.Format(" {0} = @{1}", i.Chave, i.Chave));
                            }
                            else
                            {
                                s.Append(String.Format(" {0} = @{1} {2}", i.Chave, i.Chave, "AND"));
                                index = index + 1;
                            }
                        }
                    }
                }
            }
            return s.ToString();
        }
        **/


        internal static string SelectAllSqlStringPorID<T>(Parameters campos)
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");

            if (campos.Propriedades != null)
            {
                s.Append(chave.Name);
                s.Append(", ");

                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(campos.Propriedades[i].ToString());
                    s.Append(", ");
                }
                s.Append(campos.Propriedades[campos.Propriedades.Count - 1].ToString());

            }
            else
            {
                s.Append(" * ");
            }
            s.Append(" FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE ");
            s.Append(campos.Criteria.Key);
            s.Append(" = @");
            s.Append(campos.Criteria.Key);

            return s.ToString();
        }

        internal static string SelectCamposSqlString<T>(Parameters campos)
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");
            if (campos.Propriedades != null)
            {
                s.Append(chave.Name);
                s.Append(", ");

                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(campos.Propriedades[i].ToString());
                    s.Append(", ");
                }
                s.Append(campos.Propriedades[campos.Propriedades.Count - 1].ToString());

            }
            else
            {
                s.Append(" * ");
            }
            s.Append(" FROM ");
            s.Append(tipo.Name);

            return s.ToString();
        }

        internal static string SelectComplexSqlString<T>(Entity entity) where T : new()
        {
            Type tipo = typeof(T);
            var propriedades = Helper.DefinirPropriedadesBasicas<T>();
            int tam = propriedades.Length;
            int index = 0;
            foreach (var et in entity.ListEntities)
            {
                var ti = et;
            }

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT ");

            foreach (var i in propriedades)
            {
                if (tam == 1)
                {
                    s.Append(String.Format(" {0}_{1} ", tipo, i.Name));
                }
                else
                {
                    if (index == 0)
                    {
                        s.Append(String.Format(" {0}_{1}, ", tipo, i.Name));
                        index = 1;
                    }
                    else
                    {
                        if (index == tam - 1)
                        {
                            s.Append(String.Format(" {0}_{1} ", tipo, i.Name));
                        }
                        else
                        {
                            s.Append(String.Format(" {0}_{1}, ", tipo, i.Name));
                            index = index + 1;
                        }
                    }
                }
            }
            s.Append(tipo);
            return s.ToString();
        }

        internal static string SelectCriteriasSqlString<T>(Parameters campos, SqlOperatorComparition operador) where T : new()
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");

            if (campos.Propriedades != null)
            {
                s.Append(chave.Name);
                s.Append(", ");

                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(campos.Propriedades[i].ToString());
                    s.Append(", ");
                }
                s.Append(campos.Propriedades[campos.Propriedades.Count - 1].ToString());
            }
            else
            {
                s.Append(" * ");
            }

            s.Append(" FROM ");
            s.Append(tipo.Name);
            if (campos.Criterias != null)
            {
                s.Append(" WHERE ");
                for (int i = 0; i < campos.Criterias.Count - 1; i++)
                {
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" = @");
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" AND ");
                }
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
                s.Append(" = @");
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
            }
            return s.ToString();
        }

        internal static string SelectEntreDuasDatasSqlString<T>(Parameters campos) where T : new()
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");
            var chave = Helper.RecuperarChavePrimaria<T>();

            if (campos.Propriedades != null)
            {
                s.Append(chave.Name);
                s.Append(", ");

                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(campos.Propriedades[i].ToString());
                    s.Append(", ");
                }
                s.Append(campos.Propriedades[campos.Propriedades.Count - 1].ToString());

            }
            else
            {
                s.Append(" * ");
            }

            s.Append(" FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE (CONVERT (nvarchar(10), ");
            s.Append(campos.DataInicial.Key);
            s.Append(", 103) between @Data1 AND @Data2) ");


            if (campos.Criterias != null)
            {
                s.Append(" AND ");
                for (int i = 0; i < campos.Criterias.Count - 1; i++)
                {
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" = @");
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" AND ");
                }
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
                s.Append(" = @");
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
            }
            return s.ToString();
        }

        internal static string SelectFullTextSqlString<T>() where T : new()
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT * FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE CONTAINS (*, @Campo)");

            return s.ToString();
        }

        internal static string SelectFullTextCriteriasSqlString<T>(Parameters campos) where T : new()
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT  ");

            if (campos.Propriedades != null)
            {
                s.Append(chave.Name);
                s.Append(", ");

                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(campos.Propriedades[i].ToString());
                    s.Append(", ");
                }
                s.Append(campos.Propriedades[campos.Propriedades.Count - 1].ToString());

            }
            else
            {
                s.Append(" * ");
            }

            s.Append(" FROM ");
            s.Append(tipo.Name);

            if (campos.Criterias != null)
            {
                s.Append(" WHERE CONTAINS (*, @Campo) AND ");
                for (int i = 0; i < campos.Criterias.Count - 1; i++)
                {
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" = @");
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" AND ");
                }
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
                s.Append(" = @");
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
            }
            else
            {
                s.Append(" WHERE CONTAINS (*, @Campo)");
            }


            return s.ToString();
        }

        internal static string UpdateComPropriedadesSqlString<T>(Parameters campos)
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append("UPDATE ");
            s.Append(tipo.Name);
            s.Append(" SET ");

            if (campos.Propriedades != null)
            {
                for (int i = 0; i < campos.Propriedades.Count - 1; i++)
                {
                    s.Append(String.Format(" {0} = @{1},", campos.Propriedades[i].ToString(), campos.Propriedades[i].ToString()));
                }
                s.Append(String.Format(" {0} = @{1}", campos.Propriedades[campos.Propriedades.Count - 1].ToString(), campos.Propriedades[campos.Propriedades.Count - 1].ToString()));
            }

            s.Append(" WHERE ");
            s.Append(" (");
            s.Append(chave.Name);
            s.Append(" = @");
            s.Append(chave.Name);
            s.Append(")");

            return s.ToString();


        }

        internal static string SelectCount<T>()
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT COUNT(ID) FROM ");
            s.Append(tipo.Name);

            return s.ToString();
        }

        internal static string SelectCountPropriedadea<T>(Parameters campos)
        {
            Type tipo = typeof(T);
            var chave = Helper.RecuperarChavePrimaria<T>();

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT COUNT(ID) FROM ");
            s.Append(tipo.Name);

            if (campos.Criterias != null)
            {
                s.Append(" WHERE ");
                for (int i = 0; i < campos.Criterias.Count - 1; i++)
                {
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" = @");
                    s.Append(campos.Criterias[i].Key);
                    s.Append(" AND ");
                }
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
                s.Append(" = @");
                s.Append(campos.Criterias[campos.Criterias.Count - 1].Key);
            }

            return s.ToString();


        }

        internal static string SelectMax<T>(string coluna)
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT MAX(");
            s.Append(coluna); 
            s.Append(") FROM ");
            s.Append(tipo.Name);

            return s.ToString();
        }

        internal static string SelectMin<T>(string coluna)
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT MIN(");
            s.Append(coluna);
            s.Append(") FROM ");
            s.Append(tipo.Name);

            return s.ToString();
        }

        internal static string SelectFirst<T>(string propriedade, string valor)
        {
            Type tipo = typeof(T);

            StringBuilder s = new StringBuilder();
            s.Append(" SELECT * FROM ");
            s.Append(tipo.Name);
            s.Append(" WHERE ");
            s.Append(propriedade);
            s.Append(" = ");
            s.Append(valor);

            return s.ToString();
        }
    }
}
