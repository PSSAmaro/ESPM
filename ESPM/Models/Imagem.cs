using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Imagens")]
    public class Imagem
    {
        // Assumindo que é possível gerar o URL da imagem a partir apenas destas informações...
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Pedido a que pertence esta imagem
        [Required]
        public virtual Pedido Pedido { get; set; }

        // Momento em que esta fotografia foi tirada
        // Se não houver informação de tempo é o momento em que a informação foi recebida
        public DateTime Tempo { get; set; }
    }
}