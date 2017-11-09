using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Tipos de resultados possíveis da validação.
    /// </summary>
    public enum Resultado
    {
        /// <summary>
        /// Pedido válido
        /// </summary>
        Valido,
        /// <summary>
        /// Formato enviado não é válido
        /// </summary>
        MauFormato,
        /// <summary>
        /// Erro de autenticação
        /// </summary>
        ErroAutenticacao,
        /// <summary>
        /// Informações enviadas insuficientes
        /// </summary>
        DadosInsuficientes,
        /// <summary>
        /// Pedido não credível
        /// </summary>
        Suspeito,
        /// <summary>
        /// Pedido repetido
        /// </summary>
        Repetido,
        /// <summary>
        /// Pedido não avaliado
        /// </summary>
        NaoAvaliado
    }

    /// <summary>
    /// Avaliação de um Request recebido e conteúdo do mesmo.
    /// </summary>
    [Table("AvaliacoesPedidos")]
    public class AvaliacaoPedido
    {
        /// <summary>
        /// ID da avaliação.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Momento em que foi efetuada a avaliação.
        /// </summary>
        [Required]
        public DateTime Tempo { get; set; }

        /// <summary>
        /// Endereço IP que enviou o pedido.
        /// </summary>
        [Required]
        public virtual Endereco Endereco { get; set; }

        /// <summary>
        /// Resultado da avaliação.
        /// </summary>
        [Required]
        public Resultado Resultado { get; set; }

        /// <summary>
        /// Header de autenticação enviado.
        /// </summary>
        public Guid? Header { get; set; }

        /// <summary>
        /// Corpo enviado.
        /// </summary>
        public string Corpo { get; set; }
    }
}