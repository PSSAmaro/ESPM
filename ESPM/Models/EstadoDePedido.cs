using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Estado de um pedido.
    /// </summary>
    [Table("EstadosDePedidos")]
    public class EstadoDePedido
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Pedido a que se refere este estado.
        /// </summary>
        [Required]
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Estado do pedido.
        /// </summary>
        [Required]
        public virtual Estado Estado { get; set; }

        /// <summary>
        /// Utilizador que fez esta mudança de estado. Null no estado inicial.
        /// </summary>
        public virtual ApplicationUser Utilizador { get; set; }

        /// <summary>
        /// Momento em que o pedido mudou para este estado.
        /// </summary>
        [Required]
        public DateTime Tempo { get; set; }
    }
}