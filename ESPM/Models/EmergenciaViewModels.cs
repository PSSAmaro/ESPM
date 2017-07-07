using ESPM.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Modelo dos pedidos enviados.
    /// </summary>
    [ModelName("Emergencia")]
    public class EmergenciaViewModel
    {
        /// <summary>
        /// Momento em que o pedido foi enviado.
        /// </summary>
        public DateTime? Tempo { get; set; }

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
        /// Outros detalhes da condição da pessoa que precisa de ajuda.
        /// </summary>
        public string OutrosDetalhesPessoa { get; set; }

        /// <summary>
        /// Descrição da situação.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Lista de localizações detetadas.
        /// </summary>
        public List<LocalizacaoViewModel> Localizacoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public EmergenciaViewModel()
        {
            Localizacoes = new List<LocalizacaoViewModel>();
        }
    }

    /// <summary>
    /// Modelo das localizações enviadas.
    /// </summary>
    [ModelName("Localizacao")]
    public class LocalizacaoViewModel
    {
        /// <summary>
        /// Momento em que a localização foi detetada.
        /// </summary>
        /// <remarks>
        /// Se não for enviado nenhum tempo, é utilizado o tempo em que esta localização é recebido.
        /// </remarks>
        public DateTime? Tempo { get; set; }

        /// <summary>
        /// Latitude da localização detetada.
        /// </summary>
        [Required]
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude da localização detetada.
        /// </summary>
        [Required]
        public float Longitude { get; set; }
    }

    /// <summary>
    /// Modelo do estado atual de um pedido.
    /// </summary>
    [ModelName("EstadoAtual")]
    public class EstadoAtualViewModel
    {
        /// <summary>
        /// Estado atual do pedido.
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Hora da última alteração do estado do pedido.
        /// </summary>
        public DateTime Modificado { get; set; }
    }

    /// <summary>
    /// Resultado do pedido enviado e o seu ID.
    /// </summary>
    // Talvez adicionar alguma mensagem
    [ModelName("Recebido")]
    public class RecebidoViewModel
    {
        /// <summary>
        /// Momento em que o pedido foi recebido.
        /// </summary>
        public DateTime Recebido { get; set; }

        /// <summary>
        /// ID do pedido criado.
        /// </summary>
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Resultado da atualização do pedido.
    /// </summary>
    [ModelName("Atualizado")]
    public class AtualizadoViewModel
    {
        /// <summary>
        /// Momento em que a atualização foi recebida.
        /// </summary>
        public DateTime Recebido { get; set; }

        /// <summary>
        /// Indica se alguma informação do pedido recebido era nova.
        /// </summary>
        public bool Atualizado { get; set; }
    }

    /// <summary>
    /// Resultado da atualização do pedido.
    /// </summary>
    [ModelName("Cancelado")]
    public class CanceladoViewModel
    {
        /// <summary>
        /// Momento o pedido de cancelamento foi recebido.
        /// </summary>
        public DateTime Recebido { get; set; }
    }
}