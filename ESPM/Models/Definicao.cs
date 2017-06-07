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
    // Serão demasiados campos? Better safe than sorry
    // Valores iniciais em DbInitializer
    [Table("Definicoes")]
    public class Definicao
    {
        [Key]
        public string Nome { get; set; }

        // Valor máximo da definição
        // O mínimo é assumido 0
        // Se não houver máximo, Maximo = 0
        [Required]
        public int Maximo { get; set; }

        [Required]
        public int Valor { get; set; }

        // Descrição da definição
        [Required]
        public string Descricao { get; set; }

        // O que significa esta definição ser 0?
        [Required]
        public string SignificadoZero { get; set; }

        // O que significa esta definição ser diferente de 0?
        [Required]
        public string SignificadoOutro { get; set; }
        
        // Lista de modificações a esta definição
        public virtual List<MudancaDefinicao> Mudancas { get; set; }
    }
}