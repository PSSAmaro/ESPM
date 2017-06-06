using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    // Definições atuais do sistema
    // Talvez haja uma forma nativa de fazer?
    // Valores iniciais em DbInitializer
    [Table("Definicoes")]
    public class Definicao
    {
        [Key]
        public string Nome { get; set; }

        [Required]
        public int Valor { get; set; }

        // Lista de modificações a esta definição
        public virtual List<MudancaDefinicao> Mudancas { get; set; }
    }
}