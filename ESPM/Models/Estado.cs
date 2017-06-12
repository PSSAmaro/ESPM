using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Estado possível para um pedido.
    /// </summary>
    [Table("Estados")]
    public class Estado
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nome do estado.
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Família do estado.
        /// </summary>
        [Required]
        public int Familia { get; set; }

        /// <summary>
        /// Define se o estado está disponível ou não.
        /// </summary>
        [Required]
        public bool Ativo { get; set; }

        /// <summary>
        /// Lista de estados dos quais é possível chegar a este estado.
        /// </summary>
        // Nome do estado anterior: Anteriores.De.Nome
        [InverseProperty("Para")]
        public virtual List<TransicaoDeEstado> Anteriores { get; set; }

        /// <summary>
        /// Lista de estados para os quais este pode transitar.
        /// </summary>
        // Nome do estado seguinte: Seguintes.Para.Nome
        [InverseProperty("De")]
        public virtual List<TransicaoDeEstado> Seguintes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Estado()
        {
            Anteriores = new List<TransicaoDeEstado>();
            Seguintes = new List<TransicaoDeEstado>();
        }
    }
}