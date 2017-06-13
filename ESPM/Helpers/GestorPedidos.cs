﻿using ESPM.Models;
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
    public class GestorPedidos
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
        public async Task<Pedido> CriarPedido(EmergenciaViewModel emergencia, Autorizacao autorizacao)
        {
            // Usar o tempo recebido ou o atual
            DateTime t = (emergencia.Tempo == null ? DateTime.Now : (DateTime)emergencia.Tempo);
            Pedido pedido = new Pedido()
            {
                Autorizacao = autorizacao,
                Tempo = t,
                Nome = emergencia.Nome,
                Contacto = emergencia.Contacto,
                Idade = emergencia.Idade,
                OutrosDetalhesPessoa = emergencia.OutrosDetalhesPessoa,
            };
            // Adicionar descrição e localizações se existirem
            if (emergencia.Descricao != null)
                pedido.Descricoes.Add(await CriarDescricao(emergencia.Descricao, t));
            if (emergencia.Localizacoes != null)
                pedido.Localizacoes.AddRange(await CriarLocalizacoes(emergencia.Localizacoes));
            // Adicionar o estado inicial
            pedido.Estados.Add(await CriarEstadoDePedido(db.Estados.Where(e => e.Anteriores.Count == 0).FirstOrDefault()));

            db.Pedidos.Add(pedido);
            await db.SaveChangesAsync();

            return pedido;
        }

        /// <summary>
        /// Marca um pedido como anulado.
        /// </summary>
        /// <param name="pedido">Pedido a anular.</param>
        /// <returns></returns>
        public async Task<EstadoDePedido> CancelarPedido(Pedido pedido)
        {
            // Adicionar o estado Anulada
            return await CriarEstadoDePedido(db.Estados.Where(e => e.Nome == "Anulada").FirstOrDefault(), pedido);
        }

        /// <summary>
        /// Insere uma ou mais novas localizações num pedido.
        /// </summary>
        /// <param name="localizacoes">Nova localização.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a localização.</param>
        /// <returns></returns>
        public async Task<List<Localizacao>> CriarLocalizacoes(List<LocalizacaoViewModel> localizacoes, Pedido pedido = null)
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

            if (pedido != null)
            {
                db.Localizacoes.AddRange(novas);
                await db.SaveChangesAsync();
            }

            return novas;
        }

        /// <summary>
        /// Cria um novo estado para um pedido.
        /// </summary>
        /// <param name="estado">Novo estado do pedido.</param>
        /// <param name="pedido">Pedido ao qual adicionar o estado.</param>
        /// <returns></returns>
        public async Task<EstadoDePedido> CriarEstadoDePedido(Estado estado, Pedido pedido = null)
        {
            EstadoDePedido novo = new EstadoDePedido()
            {
                Pedido = pedido,
                Estado = estado,
                Tempo = DateTime.Now
            };

            if (pedido != null)
            {
                db.EstadosDePedido.Add(novo);
                await db.SaveChangesAsync();
            }

            return novo;
        }

        /// <summary>
        /// Cria uma nova descrição para um pedido.
        /// </summary>
        /// <param name="texto">Texto da descrição.</param>
        /// <param name="tempo">Momento em que a descrição foi escrita.</param>
        /// <param name="pedido">Pedido ao qual deve ser adicionada a descrição.</param>
        /// <returns></returns>
        public async Task<Descricao> CriarDescricao(string texto, DateTime tempo, Pedido pedido = null)
        {
            if (texto == null)
                return null;

            Descricao descricao = new Descricao()
            {
                Pedido = pedido,
                Tempo = tempo,
                Texto = texto
            };

            if (pedido != null)
            {
                db.Descricoes.Add(descricao);
                await db.SaveChangesAsync();
            }

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
        /// Libertar recursos.
        /// </summary>
        // Deve ser importante
        public void Dispose()
        {
            db.Dispose();
        }
    }
}