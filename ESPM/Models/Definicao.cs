using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Definição do sistema.
    /// </summary>
    // Talvez haja uma forma nativa de fazer?
    // Serão demasiados campos? Better safe than sorry
    // Valores iniciais em DbInitializer
    [Table("Definicoes")]
    public class Definicao
    {
        /// <summary>
        /// Nome da definição.
        /// </summary>
        [Key]
        public string Nome { get; set; }

        /// <summary>
        /// Valor máximo da definição.
        /// </summary>
        // Se não houver máximo, Maximo = 0
        [Required]
        public int Maximo { get; set; }

        /// <summary>
        /// Valor atual da definição.
        /// </summary>
        [Required]
        public int Valor { get; set; }

        /// <summary>
        /// Nome de apresentação da definição.
        /// </summary>
        [Required]
        public string Apresentacao { get; set; }

        /// <summary>
        /// Descrição da definição.
        /// </summary>
        [Required]
        public string Descricao { get; set; }

        /// <summary>
        /// O que significa esta definição ser 0?
        /// </summary>
        [Required]
        public string SignificadoZero { get; set; }

        /// <summary>
        /// O que significa esta definição ser diferente de 0?
        /// </summary>
        [Required]
        public string SignificadoOutro { get; set; }

        /// <summary>
        /// Lista de modificações a esta definição.
        /// </summary>
        public virtual List<AlteracaoDefinicao> Alteracoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Definicao()
        {
            Alteracoes = new List<AlteracaoDefinicao>();
        }
    }
}