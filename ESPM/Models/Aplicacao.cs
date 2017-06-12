using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Aplicação autorizada.
    /// </summary>
    [Table("Aplicacoes")]
    public class Aplicacao
    {
        /// <summary>
        /// ID da aplicação. Enviada com cada pedido.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Utilizador responsável por esta aplicação, com acesso ao sistema de teste.
        /// </summary>
        [Required]
        public virtual ApplicationUser Utilizador { get; set; }

        /// <summary>
        /// Nome da aplicação.
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Lista de autorizações da aplicação.
        /// </summary>
        public virtual List<Autorizacao> Autorizacoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Aplicacao()
        {
            Autorizacoes = new List<Autorizacao>();
        }
    }
}