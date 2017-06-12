using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Autorização de aplicação.
    /// </summary>
    [Table("Autorizacoes")]
    public class Autorizacao
    {
        /// <summary>
        /// ID da autorização. Esta chave é a usada pela aplicação para autenticação.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Aplicação a que se refere a autorização.
        /// </summary>
        [Required]
        public Aplicacao Aplicacao { get; set; }

        /// <summary>
        /// Validade da autorização.
        /// </summary>
        [Required]
        public DateTime Validade { get; set; }

        /// <summary>
        /// Define se é uma autorização real ou de teste.
        /// </summary>
        [Required]
        public bool Teste { get; set; }

        /// <summary>
        /// Define se esta autorização foi revogada.
        /// </summary>
        [Required]
        public bool Revogada { get; set; }
    }
}