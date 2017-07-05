using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Imagem de um pedido.
    /// </summary>
    [Table("Imagens")]
    public class Imagem
    {
        /// <summary>
        /// ID.
        /// </summary>
        // Assumindo que é possível gerar o URL da imagem a partir apenas destas informações...
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Request de onde foi enviada a imagem.
        /// </summary>
        [Required]
        public virtual Avaliacao Avaliacao { get; set; }

        /// <summary>
        /// Pedido a que pertence esta imagem.
        /// </summary>
        [Required]
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Momento em que esta fotografia foi tirada.
        /// </summary>
        // Se não houver informação de tempo é o momento em que a informação foi recebida
        public DateTime Tempo { get; set; }
    }
}