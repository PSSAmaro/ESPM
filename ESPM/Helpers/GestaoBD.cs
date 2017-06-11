using ESPM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ESPM.Helpers
{
    /// <summary>
    /// Gere a comunicação dos controladores com a Base de Dados.
    /// </summary>
    // Toda esta classe provavelmente devia ser implementada com os get e set do C#, mas eu sou mau e nem procurei se dava.
    public class GestaoBD
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Devolve o pedido com o id definido.
        /// Devolve null se não for encontrado.
        /// </summary>
        /// <param name="id">ID do pedido.</param>
        public async Task<Pedido> LerPedido(Guid id)
        {
            return await db.Pedidos.FindAsync(id);
        }

        /// <summary>
        /// Inserir um novo pedido na base de dados.
        /// </summary>
        /// <param name="emergencia">A emergência recebida.</param>
        /// <param name="autorizacao">A autorização utilizada pela aplicação.</param>
        /// <returns></returns>
        // Falta guardar os pedidos com erro... ups
        public Pedido CriarPedido(EmergenciaViewModel emergencia, Autorizacao autorizacao)
        {
            DateTime t = DateTime.Now;
            Pedido pedido = new Pedido()
            {
                // Associar a autorização usada
                Autorizacao = autorizacao,
                // Usar o tempo recebido ou o atual
                Tempo = (emergencia.Tempo == null ? t : (DateTime)emergencia.Tempo),
                Nome = emergencia.Nome,
                Contacto = emergencia.Contacto,
                Idade = emergencia.Idade,
                OutrosDetalhesPessoa = emergencia.OutrosDetalhesPessoa
            };

            // Adicionar descrição se existir descrição
            if (emergencia.Descricao != null)
                CriarDescricao(emergencia.Descricao, pedido, pedido.Tempo);

            // Adicionar localização se existir localização
            if (emergencia.Localizacoes != null)
                CriarLocalizacoes(emergencia.Localizacoes, pedido);

            // Criar o estado inicial
            CriarEstadoDePedido(db.Estados.Where(e => e.Anteriores.Count == 0).FirstOrDefault(), pedido);

            // Adicionar o novo pedido à BD
            db.Pedidos.Add(pedido);

            return pedido;
        }

        /// <summary>
        /// Marca um pedido como anulado.
        /// </summary>
        /// <param name="pedido">Pedido a anular.</param>
        /// <returns></returns>
        public EstadoDePedido CancelarPedido(Pedido pedido)
        {
            // Adicionar o estado Anulada
            return CriarEstadoDePedido(db.Estados.Where(e => e.Nome == "Anulada").FirstOrDefault(), pedido);
        }

        /// <summary>
        /// Insere uma ou mais novas localizações num pedido.
        /// </summary>
        /// <param name="localizacoes">Nova localização.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a localização.</param>
        /// <returns></returns>
        public List<Localizacao> CriarLocalizacoes(List<LocalizacaoViewModel> localizacoes, Pedido pedido)
        {
            List<Localizacao> novas = new List<Localizacao>();
            foreach (var localizacao in localizacoes)
            {
                novas.Add(new Localizacao()
                {
                    Pedido = pedido,
                    // Usar o tempo recebido ou o atual
                    Tempo = (localizacao.Tempo == null ? DateTime.Now : (DateTime)localizacao.Tempo),
                    Latitude = localizacao.Latitude,
                    Longitude = localizacao.Longitude
                });
            }
            db.Localizacoes.AddRange(novas);

            return novas;
        }

        /// <summary>
        /// Cria um novo estado para um pedido.
        /// </summary>
        /// <param name="estado">Novo estado do pedido.</param>
        /// <param name="pedido">Pedido ao qual adicionar o estado.</param>
        /// <returns></returns>
        public EstadoDePedido CriarEstadoDePedido(Estado estado, Pedido pedido)
        {
            EstadoDePedido novo = new EstadoDePedido()
            {
                Pedido = pedido,
                Estado = estado,
                Tempo = DateTime.Now
            };
            db.EstadosDePedido.Add(novo);

            return novo;
        }

        /// <summary>
        /// Cria uma nova descrição para um pedido.
        /// </summary>
        /// <param name="texto">Texto da descrição.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a descrição.</param>
        /// <param name="tempo">Momento em que a descrição foi escrita.</param>
        /// <returns></returns>
        public Descricao CriarDescricao(string texto, Pedido pedido, DateTime tempo)
        {
            Descricao descricao = new Descricao()
            {
                Pedido = pedido,
                Tempo = tempo,
                Texto = texto
            };
            db.Descricoes.Add(descricao);

            return descricao;
        }

        /// <summary>
        /// Devolve a autorização para a aplicação definida.
        /// Devolve null se não for encontrada.
        /// </summary>
        /// <param name="aplicacao">ID da aplicação.</param>
        /// <param name="teste">Define se é uma autorização de teste ou real.</param>
        public Autorizacao LerAutorizacao(Guid aplicacao, bool teste = false)
        {
            return db.Autorizacoes.Where(a => a.Aplicacao.Id == aplicacao && a.Teste == teste && !a.Revogada && a.Validade > DateTime.Now).FirstOrDefault();
        }

        /// <summary>
        /// Guarda as alterações efetuadas na BD.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GuardarAlteracoes()
        {
            return await db.SaveChangesAsync();
        }

        /// <summary>
        /// Libertar recursos.
        /// </summary>
        // Deve ser importante
        public void Dispose()
        {
            db.Dispose();
        }
    }
}