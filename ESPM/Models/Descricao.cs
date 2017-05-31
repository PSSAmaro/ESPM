using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Descricoes")]
    public class Descricao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Pedido a que se refere esta descrição
        [Required]
        public virtual Pedido Pedido { get; set; }

        // Utilizador que fez esta mudança de estado
        public virtual ApplicationUser Utilizador { get; set; }

        // Momento em que a descrição foi modificada para esta versão
        public DateTime Tempo { get; set; }

        // Texto da descrição
        [Required]
        public string Texto { get; set; }
    }
}