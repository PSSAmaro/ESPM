using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Descrição de um pedido.
    /// </summary>
    [Table("Descricoes")]
    public class Descricao
    {
        /// <summary>
        /// ID da descrição.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Request de onde foi enviada a descrição.
        /// </summary>
        public virtual Avaliacao Avaliacao { get; set; }

        /// <summary>
        /// Pedido a que se refere esta descrição.
        /// </summary>
        [Required]
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Operador que fez esta mudança de descrição (Se foi feita por um operador).
        /// </summary>
        public virtual ApplicationUser Utilizador { get; set; }

        /// <summary>
        /// Momento em que a descrição foi modificada para esta versão.
        /// </summary>
        public DateTime Tempo { get; set; }

        /// <summary>
        /// Texto da descrição.
        /// </summary>
        [Required]
        public string Texto { get; set; }
    }
}