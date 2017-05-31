using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("EstadosDePedidos")]
    public class EstadoDePedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Pedido a que se refere este estado
        [Required]
        public virtual Pedido Pedido { get; set; }

        [Required]
        public virtual Estado Estado { get; set; }

        // Utilizador que fez esta mudança de estado
        public virtual ApplicationUser Utilizador { get; set; }

        // Momento em que o pedido mudou para este estado
        [Required]
        public DateTime Tempo { get; set; }
    }
}