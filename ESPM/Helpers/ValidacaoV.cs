using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ESPM.Helpers
{
    /// <summary>
    /// Validação de todos os requests recebidos, incluindo autorizações.
    /// </summary>
    // FALTA: Criar uma tabela com as avaliações desta classe
    public class ValidacaoV
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
        /// <param name="headers">O header com o hash.</param>
        public ValidacaoV(EmergenciaViewModel emergencia, Autorizacao autorizacao, HttpRequestHeaders headers)
        {
            // FALTA: Definir a credibilidade do pedido
            // FALTA: Se for enviada mais de 1 localização, estas devem ter tempo

            // Apesar do modelo válido convém confirmar que foram enviadas informações suficientes
            // Se todos os seguintes campos forem nulos, é considerado que não há informações suficientes
            if (emergencia.Contacto == null && emergencia.OutrosDetalhesPessoa == null && emergencia.Descricao == null && (emergencia.Localizacoes == null || emergencia.Localizacoes.Count == 0))
                Resultado = Resultado.DadosInsuficientes;

            // Se não foi encontrada nenhuma autorização válida para a aplicação usada ou o header hash é inexistente/inválido
            /* else if (autorizacao == null || headers.GetValues("Hash") == null || headers.GetValues("Hash").First() != Hash(emergencia.ToString(), autorizacao.Id))
                Resultado = Resultado.ErroAutenticacao; */

            else
                Resultado = Resultado.Valido;
        }

        /// <summary>
        /// Construtor usado aquando do envio de uma atualização.
        /// </summary>
        /// <param name="atualizacao">A nova localização.</param>
        /// <param name="pedido">A autorização da aplicação.</param>
        /// <param name="headers">O header com o hash.</param>
        public ValidacaoV(AtualizacaoViewModel atualizacao, Pedido pedido, HttpRequestHeaders headers)
        {
            // Se não existir pelo menos 1 informação atualizada
            if (atualizacao.Descricao == null && (atualizacao.Localizacoes == null || atualizacao.Localizacoes.Count == 0))
                Resultado = Resultado.DadosInsuficientes;
            
            // Se não foi encontrada nenhuma autorização válida para a aplicação usada ou o header hash é inexistente/inválido
            // string: Pedido.id + atualizacao.ToString()
            /* else if (pedido == null || headers.GetValues("Hash") == null || headers.GetValues("Hash").First() != Hash(pedido.Id + atualizacao.ToString(), pedido.Autorizacao.Id))
                Resultado = Resultado.ErroAutenticacao; */

            else
                Resultado = Resultado.Valido;
        }

        /// <summary>
        /// Construtor usado aquando do pedido de cancelamento.
        /// </summary>
        /// <param name="pedido">O pedido a adicionar a localização.</param>
        /// <param name="headers">O header com o hash.</param>
        public ValidacaoV(Pedido pedido, HttpRequestHeaders headers)
        {
            /* if (headers.GetValues("Hash") == null || headers.GetValues("Hash").First() != Hash(pedido.Id.ToString(), pedido.Autorizacao.Id))
                Resultado = Resultado.ErroAutenticacao;

            else */
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