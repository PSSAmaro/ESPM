using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Alteração às definições do sistema.
    /// </summary>
    [Table("AlteracoesDefinicoes")]
    public class AlteracaoDefinicao
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Utilizador que fez a mudança.
        /// </summary>
        [Required]
        public virtual ApplicationUser Utilizador { get; set; }

        /// <summary>
        /// Definição editada.
        /// </summary>
        [Required]
        public virtual Definicao Definicao { get; set; }

        /// <summary>
        /// Valor anterior.
        /// </summary>
        [Required]
        public int De { get; set; }

        /// <summary>
        /// Novo valor.
        /// </summary>
        [Required]
        public int Para { get; set; }

        /// <summary>
        /// Momento em que a edição foi feita.
        /// </summary>
        [Required]
        public DateTime Tempo { get; set; }
    }
}