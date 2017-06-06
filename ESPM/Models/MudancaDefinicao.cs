using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    // Log das alterações às definições do sistema
    [Table("MudancasDefinicoes")]
    public class MudancaDefinicao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Utilizador que fez a mudança
        [Required]
        public virtual ApplicationUser Utilizador { get; set; }

        // Definição editada
        [Required]
        public virtual Definicao Definicao { get; set; }

        // Valor anterior
        [Required]
        public int De { get; set; }

        // Novo valor
        [Required]
        public int Para { get; set; }

        // Momento em que a edição foi feita
        [Required]
        public DateTime Tempo { get; set; }
    }
}