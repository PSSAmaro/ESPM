using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ESPM.Helpers
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
        Repetido
    }

    /// <summary>
    /// Validação de todos os requests recebidos, incluindo autorizações.
    /// </summary>
    // FALTA: Criar uma tabela com as avaliações desta classe
    public class Validacao
    {
        /// <summary>
        /// Guarda o resultado da validação.
        /// </summary>
        public Resultado Resultado;

        /// <summary>
        /// Lista de mensagens de erro para os tipos de resultados.
        /// </summary>
        public string[] Mensagem = new string[5]
        {
            "Pedido válido",
            "Erro de autenticação",
            "Informações insuficientes",
            "Pedido bloqueado",
            "Pedido já enviado"
        };

        /// <summary>
        /// Construtor usado aquando da criação de um novo pedido.
        /// </summary>
        /// <param name="emergencia">A emergência a validar.</param>
        /// <param name="autorizacao">A autorização da aplicação.</param>
        /// <param name="hash">O header com o hash.</param>
        public Validacao(EmergenciaViewModel emergencia, Autorizacao autorizacao, IEnumerable<string> hash)
        {
            // Se não foi encontrada nenhuma autorização válida para a aplicação usada ou o header hash é inexistente/inválido
            if (autorizacao == null || hash == null || hash.First() != Hash(emergencia.ToString(), autorizacao.Id))
            {
                Resultado = Resultado.ErroAutenticacao;
                return;
            }

            // Apesar do modelo válido convém confirmar que foram enviadas informações suficientes
            // Se todos os seguintes campos forem nulos, é considerado que não há informações suficientes
            // FALTA: Se for enviada mais de 1 localização, estas devem ter tempo
            if (emergencia.Contacto == null && emergencia.OutrosDetalhesPessoa == null && emergencia.Descricao == null && !(emergencia.Latitude != null && emergencia.Longitude != null))
            {
                Resultado = Resultado.DadosInsuficientes;
                return;
            }

            // FALTA: Definir a credibilidade do pedido

            // Se chegou aqui, o pedido é válido
            Resultado = Resultado.Valido;
        }

        /// <summary>
        /// Construtor usado aquando do envio de uma nova localização.
        /// </summary>
        /// <param name="localizacoes">A nova localização.</param>
        /// <param name="autorizacao">A autorização da aplicação.</param>
        /// <param name="hash">O header com o hash.</param>
        public Validacao(List<LocalizacaoViewModel> localizacoes, Autorizacao autorizacao, IEnumerable<string> hash)
        {
            // Se não foi encontrada nenhuma autorização válida para a aplicação usada ou o header hash é inexistente/inválido
            if (autorizacao == null || hash == null || hash.First() != Hash(string.Join("", localizacoes), autorizacao.Id))
            {
                Resultado = Resultado.ErroAutenticacao;
                return;
            }

            // TALVEZ: Impor um limite de distância por tempo?

            // Se chegou aqui, o pedido é válido
            Resultado = Resultado.Valido;
        }

        /// <summary>
        /// Construtor usado aquando do pedido de cancelamento.
        /// </summary>
        /// <param name="pedido">O pedido a adicionar a localização.</param>
        /// <param name="hash">O header com o hash.</param>
        public Validacao(Pedido pedido, IEnumerable<string> hash)
        {
            if (hash == null || hash.First() != Hash(pedido.Id.ToString(), pedido.Autorizacao.Id))
            {
                Resultado = Resultado.ErroAutenticacao;
                return;
            }

            // Se chegou aqui, o pedido é válido
            Resultado = Resultado.Valido;
        }

        private string Hash(string s, Guid g)
        {
            string str = s + g;
            // https://stackoverflow.com/questions/17292366/hashing-with-sha1-algorithm-in-c-sharp
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
    }
}