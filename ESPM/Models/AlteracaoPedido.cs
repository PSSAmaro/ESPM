using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    public enum Campo
    {
        Operador,
        Estado,
        Nome,
        Contacto,
        Idade,
        OutrosDetalhes,
        Descricao,
        Localizacao,
        Imagem
    }

    [Table("AlteracoesPedidos")]
    public class AlteracaoPedido
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Momento em que a edição foi feita.
        /// </summary>
        [Required]
        public DateTime Tempo { get; private set; }

        /// <summary>
        /// Pedido a que foi efetuada a alteração.
        /// </summary>
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Utilizador que fez a mudança.
        /// </summary>
        public virtual ApplicationUser Utilizador { get; private set; }

        /// <summary>
        /// Campo editado.
        /// </summary>
        [Required]
        public Campo Campo { get; private set; }

        /// <summary>
        /// Valor do campo.
        /// </summary>
        [Required]
        public string Valor { get; private set; }

        /// <summary>
        /// Avaliação do pedido onde foi recebido esta alteração.
        /// </summary>
        public bool AvaliacaoValor { get; private set; }

        public AlteracaoPedido(Campo campo, string valor, bool avaliacao, ApplicationUser utilizador = null)
        {
            Utilizador = utilizador;
            Campo = campo;
            Valor = valor;
            Tempo = DateTime.Now;
            // O valor é validado mesmo que venha de um operador, mas pode não ser mostrado
            AvaliacaoValor = avaliacao;
        }

        public AlteracaoPedido() { }
    }
}