using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Transição possível entre estados de pedidos.
    /// </summary>
    [Table("TransicoesDeEstado")]
    public class TransicaoDeEstado
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Estado antes desta transição.
        /// </summary>
        [Required]
        public virtual Estado De { get; set; }

        /// <summary>
        /// Estado depois desta transição.
        /// </summary>
        [Required]
        public virtual Estado Para { get; set; }

        /// <summary>
        /// Define se a transição está ativa.
        /// </summary>
        [Required]
        public bool Ativa { get; set; }
    }
}