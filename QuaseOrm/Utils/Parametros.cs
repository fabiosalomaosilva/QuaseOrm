using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuaseOrm.Utils
{
    public class Parameters
    {
        public Parameters()
        {

        }

        public Parameters(Criteria Criteria)
        {
            this.Criteria = Criteria;
        }

        public Parameters(IList<Criteria> Criterias)
        {
            IList<Criteria> c = Criterias;
            this.Criterias = c;
        }
        /// <summary>
        /// Adiciona um critério tipo chave/valor a ser utilizado em updates, deletes e finds
        /// </summary>
        public Criteria Criteria { get; set; }

        /// <summary>
        /// Adiciona uma propriedade a ser utilizado consultas
        /// </summary>
        public string Propriedade { get; set; }

        /// <summary>
        /// Adiciona uma campo para fusão de tabelas relacionais
        /// </summary>
        //public IList<string> Joins { get; set; }

        /// <summary>
        /// Data para de início da consulta
        /// </summary>
        public Criteria DataInicial { get; set; }
        /// <summary>
        /// Data fim da consulta
        /// </summary>
        public Criteria DataFinal { get; set; }


        /// <summary>
        /// Adiciona uma propriedade a ser utilizado consultas
        /// </summary>
        public object ParametroID { get; set; }

        /// <summary>
        /// Lista de propriedades a ser retornadas no Select
        /// </summary>
        public IList<string> Propriedades { get; set; }

        /// <summary>
        /// Lista de critérios tipo chave/valor a ser pesquisados no Select
        /// </summary>
        public IList<Criteria> Criterias { get; set; }

        /// <summary>
        /// Adicionar nova propriedade à lista
        /// </summary>
        public void AddPropriedades(string propriedade)
        {
            IList<string> c;
            if (Propriedades == null)
            {
                c = new List<string>();
            }
            else
            {
                c = Propriedades;
            }

            c.Add(propriedade);
            Propriedades = c;
        }

        /**
        /// <summary>
        /// Adicionar novo join ao select
        /// </summary>
        public void AddJoin(string Entity)
        {
            IList<string> c;
            if (Joins == null)
            {
                c = new List<string>();
            }
            else
            {
                c = Joins;
            }

            c.Add(Entity);
            Joins = c;
        }
        **/

        /// <summary>
        /// Adicionar novo critério à lista
        /// </summary>
        public void AddCriteria(string chave, object valor)
        {
            try
            {
                IList<Criteria> c;
                if (Criterias == null)
                {
                    c = new List<Criteria>();
                }
                else
                {
                    c = Criterias;
                }
                
                c.Add(new Criteria(chave, valor));
                Criterias = c;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adicionar nova data para select simples ou entre duas datas 
        /// </summary>
        public void AddDataInicial(string chave, object valor)
        {
            try
            {
                DataInicial = new Criteria(chave, valor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adicionar data de fim para select com duas datas
        /// </summary>
        public void AddDataFinal(string chave, object valor)
        {
            try
            {
                DataFinal = new Criteria(chave, valor);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

    }
}