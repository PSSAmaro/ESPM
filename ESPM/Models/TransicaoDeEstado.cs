using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("TransicoesDeEstado")]
    public class TransicaoDeEstado
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Estado De { get; set; }

        [Required]
        public virtual Estado Para { get; set; }

        [Required]
        public bool Ativa { get; set; }
    }
}