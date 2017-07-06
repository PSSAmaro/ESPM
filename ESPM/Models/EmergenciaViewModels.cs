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
    /// Pedido de atualização de uma emergência.
    /// </summary>
    [ModelName("Atualizacao")]
    public class AtualizacaoViewModel
    {
        /// <summary>
        /// Momento em que a atualização foi enviada.
        /// </summary>
        /// <remarks>
        /// Opcionalmente, caso seja enviada apenas 1 foto com a sua localização, este campo pode incluir o momento em que a foto foi tirada, de modo a não ser necessário repetir o mesmo tempo em ambos os campos.
        /// </remarks>
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
        /// Descrição atualizada da situação.
        /// </summary>
        /// <remarks>
        /// Deve ser uma atualização da descrição anterior e não uma nova descrição.
        /// </remarks>
        public string Descricao { get; set; }

        /// <summary>
        /// Lista de novas localizações detetadas.
        /// </summary>
        public List<LocalizacaoViewModel> Localizacoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public AtualizacaoViewModel()
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
    /// Resultado do pedido enviado e o seu ID.
    /// </summary>
    // Talvez adicionar alguma mensagem
    [ModelName("Resultado")]
    public class RecebidoViewModel
    {
        /// <summary>
        /// ID do pedido criado.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Construtor do ViewModel.
        /// </summary>
        /// <param name="id">ID do novo pedido.</param>
        public RecebidoViewModel(Guid id)
        {
            Id = id;
        }
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

        /// <summary>
        /// Construtor do ViewModel.
        /// </summary>
        /// <param name="estado">Estado mais recente do pedido.</param>
        public EstadoAtualViewModel(EstadoDePedido estado)
        {
            Estado = estado.Estado.Nome;
            Modificado = estado.Tempo;
        }
    }
}