using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Autorizacoes")]
    public class Autorizacao
    {
        // Esta chave é a usada pela aplicação para autenticação
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Aplicacao Aplicacao { get; set; }

        public DateTime Validade { get; set; }

        [Required]
        public bool Teste { get; set; }

        [Required]
        public bool Revogada { get; set; }

        public bool Valida()
        {
            // Verificar se funciona
            if (Validade == null)
                return !Revogada;
            return DateTime.Now < Validade && !Revogada;
        }
    }
}