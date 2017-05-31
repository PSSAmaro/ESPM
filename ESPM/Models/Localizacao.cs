using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Localizacoes")]
    public class Localizacao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Pedido a que se refere esta localização
        [Required]
        public virtual Pedido Pedido { get; set; }

        // Momento em que esta localização foi detetada
        // Se não houver informação de tempo é o momento em que a informação foi recebida
        public DateTime Tempo { get; set; }

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }
    }
}