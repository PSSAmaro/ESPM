using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        public virtual AvaliacaoPedido Avaliacao { get; set; }

        /// <summary>
        /// Momento em que o pedido foi enviado.
        /// </summary>
        // Se não houver informação de tempo no pedido é o momento em que é recebido
        [Required]
        public DateTime Tempo { get; set; }

        /// <summary>
        /// Momento em que o pedido foi modificado pela última vez.
        /// </summary>
        public DateTime Modificado => Alteracoes.OrderByDescending(a => a.Tempo).First().Tempo;

        /// <summary>
        /// Nome da pessoa que precisa de ajuda.
        /// </summary>
        public string Nome { get; private set; }

        public bool SetNome(string nome, ApplicationUser utilizador = null)
        {
            if (!string.IsNullOrEmpty(nome) && Nome != nome)
            {
                Nome = nome;
                Alteracoes.Add(new AlteracaoPedido(Campo.Nome, nome, true, utilizador));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Contacto da pessoa que precisa de ajuda.
        /// </summary>
        public int? Contacto { get; private set; }

        public bool SetContacto(int? contacto, ApplicationUser utilizador = null)
        {
            if (contacto != null && contacto != 0 && Contacto != contacto)
            {
                Contacto = contacto;
                Alteracoes.Add(new AlteracaoPedido(Campo.Contacto, contacto.ToString(), true, utilizador));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Idade da pessoa que precisa de ajuda.
        /// </summary>
        public int? Idade { get; private set; }

        public bool SetIdade(int? idade, ApplicationUser utilizador = null)
        {
            if (idade != null && idade != 0 && Idade != idade)
            {
                Idade = idade;
                Alteracoes.Add(new AlteracaoPedido(Campo.Idade, idade.ToString(), true, utilizador));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Outras informações relevantes sobre a pessoa que precisa de ajuda.
        /// </summary>
        public string OutrosDetalhesPessoa { get; private set; }

        public bool SetOD(string od, ApplicationUser utilizador = null)
        {
            if (!string.IsNullOrEmpty(od) && OutrosDetalhesPessoa != od)
            {
                OutrosDetalhesPessoa = od;
                Alteracoes.Add(new AlteracaoPedido(Campo.OutrosDetalhes, od, true, utilizador));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Histórico de descrições do pedido de ajuda.
        /// </summary>
        public virtual string Descricao { get; private set; }

        public bool SetDescricao(string descricao, ApplicationUser utilizador = null)
        {
            if (descricao != null && Descricao != descricao)
            {
                Descricao = descricao;
                Alteracoes.Add(new AlteracaoPedido(Campo.Descricao, descricao, true, utilizador));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Lista de localizações recebidas.
        /// </summary>
        public virtual List<Localizacao> Localizacoes { get; private set; }

        public void SetLocalizacao(LocalizacaoViewModel localizacao, AvaliacaoPedido avaliacao)
        {
            if (localizacao != null)
            {
                Localizacoes.Add(new Localizacao(localizacao));
                Alteracoes.Add(new AlteracaoPedido(Campo.Localizacao, localizacao.Latitude + ", " + localizacao.Longitude, true));
            }
        }

        /// <summary>
        /// Localização mais recente.
        /// </summary>
        public Localizacao LocalizacaoAtual => Localizacoes.OrderByDescending(l => l.Tempo).First();

        /// <summary>
        /// Lista de imagens recebidas.
        /// </summary>
        public virtual List<Imagem> Imagens { get; set; }

        public void SetImagem(ImagemViewModel imagem, AvaliacaoPedido avaliacao)
        {
            if (imagem != null)
            {
                Imagens.Add(new Imagem(imagem));
                Alteracoes.Add(new AlteracaoPedido(Campo.Imagem, imagem.Descricao ?? "Sem descrição", true));
            }
        }

        /// <summary>
        /// Lista de estados pelos quais o pedido já passou.
        /// </summary>
        public virtual Estado EstadoAtual { get; private set; }

        public bool Fechado { get; private set; }

        public bool SetEstado(Estado estado, ApplicationUser utilizador = null)
        {
            if (utilizador == null || (EstadoAtual != estado && EstadoAtual.Seguintes.Exists(e => e.Para == estado && e.Para.Ativo)))
            {
                DateTime tempo = DateTime.Now;
                EstadoAtual = estado;
                Fechado = estado.Final;
                Alteracoes.Add(new AlteracaoPedido(Campo.Estado, estado.Nome, true, utilizador));
                return true;
            }
            return false;
        }

        public DateTime EstadoModificado => Alteracoes.OrderByDescending(a => a.Tempo).Where(a => a.Campo == Campo.Estado).First().Tempo;

        public virtual List<AlteracaoPedido> Alteracoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Pedido(EmergenciaViewModel emergencia, Estado inicial, AvaliacaoPedido avaliacao, Autorizacao autorizacao)
        {
            Localizacoes = new List<Localizacao>();
            Imagens = new List<Imagem>();
            Alteracoes = new List<AlteracaoPedido>();

            Autorizacao = autorizacao;
            Avaliacao = avaliacao;
            Tempo = emergencia.Tempo ?? DateTime.Now;
            SetNome(emergencia.Nome);
            SetContacto(emergencia.Contacto);
            SetIdade(emergencia.Idade);
            SetOD(emergencia.OutrosDetalhesPessoa);
            SetDescricao(emergencia.Descricao);
            foreach (LocalizacaoViewModel localizacao in emergencia.Localizacoes)
                SetLocalizacao(localizacao, avaliacao);
            foreach (ImagemViewModel imagem in emergencia.Imagens)
                SetImagem(imagem, avaliacao);
            SetEstado(inicial);
        }

        public Pedido()
        {
            Localizacoes = new List<Localizacao>();
            Imagens = new List<Imagem>();
            Alteracoes = new List<AlteracaoPedido>();
        }
    }
}