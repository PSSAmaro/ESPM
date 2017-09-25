using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Pedido de ajuda recebido.
    /// </summary>
    // Talvez adicionar um campo bool Aberto
    [Table("Pedidos")]
    public class Pedido
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Autorização utilizada para o pedido.
        /// </summary>
        // É utilizada a autorização em vez da aplicação para ser mais fácil distinguir pedidos válidos, pedidos inválidos e pedidos de teste
        public virtual Autorizacao Autorizacao { get; set; }

        /// <summary>
        /// Request de onde foi enviado o pedido original.
        /// </summary>
        [Required]
        public virtual Avaliacao Avaliacao { get; set; }

        /// <summary>
        /// Momento em que o pedido foi enviado.
        /// </summary>
        // Se não houver informação de tempo no pedido é o momento em que é recebido
        [Required]
        public DateTime Tempo { get; set; }

        /// <summary>
        /// Momento em que o pedido foi modificado pela última vez.
        /// </summary>
        // Atualizar sempre que alguma coisa for modificada
        [Required]
        public DateTime Modificado { get; set; }

        /// <summary>
        /// Operador responsável pelo tratamento deste pedido.
        /// </summary>
        public virtual ApplicationUser Operador { get; set; }

        /// <summary>
        /// Informação da pessoa que precisa de ajuda.
        /// </summary>
        public virtual List<Pessoa> InformacaoPessoa { get; set; }

        /// <summary>
        /// Histórico de descrições do pedido de ajuda.
        /// </summary>
        // Descrição atual: Descricoes.OrderByDescending(d => d.Tempo).FirstOrDefault()
        public virtual List<Descricao> Descricoes { get; set; }

        /// <summary>
        /// Lista de localizações recebidas.
        /// </summary>
        public virtual List<Localizacao> Localizacoes { get; set; }

        /// <summary>
        /// Lista de imagens recebidas.
        /// </summary>
        public virtual List<Imagem> Imagens { get; set; }

        /// <summary>
        /// Lista de estados pelos quais o pedido já passou.
        /// </summary>
        public virtual List<EstadoDePedido> Estados { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Pedido()
        {
            InformacaoPessoa = new List<Pessoa>();
            Descricoes = new List<Descricao>();
            Localizacoes = new List<Localizacao>();
            Imagens = new List<Imagem>();
            Estados = new List<EstadoDePedido>();
        }
    }
}