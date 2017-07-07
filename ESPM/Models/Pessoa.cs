using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Descrição da pessoa que precisa de ajuda.
    /// </summary>
    public class Pessoa
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Request de onde foi enviada a descrição da pessoa.
        /// </summary>
        public virtual Avaliacao Avaliacao { get; set; }

        /// <summary>
        /// Pedido a que se refere esta descrição da pessoa.
        /// </summary>
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Operador que fez esta mudança de descrição da pessoa (Se foi feita por um operador).
        /// </summary>
        public virtual ApplicationUser Utilizador { get; set; }

        /// <summary>
        /// Momento em que a descrição da pessoa foi modificada para esta versão.
        /// </summary>
        public DateTime Tempo { get; set; }

        /// <summary>
        /// Nome da pessoa que precisa de ajuda.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Contacto da pessoa que precisa de ajuda.
        /// </summary>
        public int? Contacto { get; set; }

        /// <summary>
        /// Idade da pessoa que precisa de ajuda.
        /// </summary>
        public int? Idade { get; set; }

        /// <summary>
        /// Outras informações relevantes sobre a pessoa que precisa de ajuda.
        /// </summary>
        public string OutrosDetalhes { get; set; }
    }
}