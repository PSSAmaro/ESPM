using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Pessoas")]
    public class Pessoa
    {
        // Adicionar mais campos?
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Pedido Pedido { get; set; }

        public string Nome { get; set; }

        // Talvez deva ser obrigatório?
        public int Contacto { get; set; }

        public int Idade { get; set; }

        // Outros detalhes da condição da pessoa
        public string Condicao { get; set; }
    }
}