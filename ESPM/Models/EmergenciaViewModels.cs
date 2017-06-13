using ESPM.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Modelo dos pedidos enviados.
    /// </summary>
    // Por enquanto não suporta imagens
    [ModelName("Pedido")]
    public class EmergenciaViewModel
    {
        /// <summary>
        /// ID da aplicação.
        /// </summary>
        public Guid Aplicacao { get; set; }

        /// <summary>
        /// Momento em que o pedido foi enviado.
        /// </summary>
        // Talvez mudar para unix time?
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

        /// <summary>
        /// Transforma o objeto em string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Modificar se o modelo for modificado
            string str = Nome + Contacto + Idade + OutrosDetalhesPessoa + Descricao + string.Join("", Localizacoes);

            return str;
        }
    }

    /// <summary>
    /// Modelo das localizações enviadas.
    /// </summary>
    // Falta o erro
    [ModelName("Localizacao")]
    public class LocalizacaoViewModel
    {
        /// <summary>
        /// Momento em que a localização foi detetada.
        /// </summary>
        public DateTime? Tempo { get; set; }

        /// <summary>
        /// Latitude da localização detetada.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude da localização detetada.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Transforma o objeto em string, removendo os caracteres "," e ".".
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Modificar se o modelo for modificado
            // Ignorar tempo porque pode ser modificado no servidor
            string str = Latitude.ToString() + Longitude;

            // Remover caracteres que podem ser ambíguos, procurar outros
            foreach (string c in new string[] { ",", "." })
                str = str.Replace(c, string.Empty);

            return str;
        }
    }

    /// <summary>
    /// Resultado do pedido enviado e o seu ID.
    /// </summary>
    // FALTA: Talvez adicionar o resultado?
    [ModelName("Resultado")]
    public class RecebidoViewModel
    {
        /// <summary>
        /// ID do pedido criado.
        /// </summary>
        public Guid Id { get; set; }
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
}