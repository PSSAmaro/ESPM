using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Lista de endereços que utilizam/utilizaram o sistema.
    /// </summary>
    [Table("Enderecos")]
    public class Endereco
    {
        /// <summary>
        /// Endereço IP.
        /// </summary>
        [Key]
        public string IP { get; set; }

        /// <summary>
        /// Indica se o IP tem carta branca (Por exemplo: Se for servidor de uma aplicação).
        /// </summary>
        [Required]
        public bool Livre { get; set; }

        /// <summary>
        /// Pedidos falsos já recebidos deste IP.
        /// </summary>
        public int Falsos { get; set; }

        /// <summary>
        /// Limite de pedidos falsos para este IP.
        /// Este valor é definido mesmo que seja um IP livre, mas é ignorado nesse caso.
        /// </summary>
        [Required]
        public int Limite { get; set; }

        /// <summary>
        /// Lista de requests recebidos deste IP.
        /// </summary>
        public virtual List<Avaliacao> Avaliacoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Endereco()
        {
            Avaliacoes = new List<Avaliacao>();
        }
    }
}