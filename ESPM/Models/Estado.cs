using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Estados")]
    public class Estado
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int Familia { get; set; }

        // Lista de estados dos quais é possível chegar a este estado
        public virtual List<Estado> Anteriores { get; set; }

        // Lista de estados para os quais este pode transitar
        public virtual List<Estado> Seguintes { get; set; }

        // Este é o estado inicial se não tiver estados anteriores (Só deve haver 1)
        public bool EstadoInicial()
        {
            return Anteriores.Count == 0;
        }

        // Este é um estado final se não tiver estados seguintes
        public bool EstadoFinal()
        {
            return Seguintes.Count == 0;
        }
    }
}