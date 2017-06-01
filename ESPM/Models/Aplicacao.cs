using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Aplicacoes")]
    public class Aplicacao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // Utilizador responsável por esta aplicação, com acesso ao sistema de teste
        [Required]
        public virtual ApplicationUser Utilizador { get; set; }

        [Required]
        public string Nome { get; set; }

        public virtual List<Autorizacao> Autorizacoes { get; set; }
    }
}