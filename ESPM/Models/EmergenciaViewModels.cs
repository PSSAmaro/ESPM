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
    [ModelName("Emergencia")]
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
        /// Lista de fotografias do local ou da situação.
        /// </summary>
        public List<ImagemViewModel> Fotografias { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public EmergenciaViewModel()
        {
            Localizacoes = new List<LocalizacaoViewModel>();
            Fotografias = new List<ImagemViewModel>();
        }

        /// <summary>
        /// Transforma o objeto em string.
        /// </summary>
        /// <returns>Concatenação de todos os campos.</returns>
        public override string ToString()
        {
            // Modificar se o modelo for modificado
            string str = Nome + Contacto + Idade + OutrosDetalhesPessoa + Descricao + string.Join("", Localizacoes);

            return str;
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
        /// Lista de novas fotografias do local ou da situação.
        /// </summary>
        public List<ImagemViewModel> Fotografias { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public AtualizacaoViewModel()
        {
            Localizacoes = new List<LocalizacaoViewModel>();
            Fotografias = new List<ImagemViewModel>();
        }

        /// <summary>
        /// Transforma o objeto em string.
        /// </summary>
        /// <returns>Concatenação de todos os campos.</returns>
        public override string ToString()
        {
            // Modificar se o modelo for modificado
            string str = Descricao + string.Join("", Localizacoes);

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
        /// <remarks>
        /// Se não for enviado nenhum tempo, é utilizado o tempo em que esta localização é recebido.
        /// Se forem enviadas várias localizações no mesmo pedido, estas devem incluir obrigatoriamente tempo de modo a ser possível criar uma linha de tempo.
        /// </remarks>
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
        /// <returns>Concatenação dos campos sem pontuação.</returns>
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
    /// Modelo das fotografias enviadas.
    /// </summary>
    [ModelName("Fotografia")]
    public class ImagemViewModel
    {
        /// <summary>
        /// Momento em que a fotografia foi tirada/recebida.
        /// </summary>
        public DateTime? Tempo { get; set; }

        /// <summary>
        /// Fotografia do local/acontecimento.
        /// </summary>
        public byte[] Imagem { get; set; }
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