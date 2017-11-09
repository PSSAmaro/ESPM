using ESPM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class ResumoPedidoViewModel
    {
        public Guid Id { get; set; }

        public ResumoEstadoViewModel Estado { get; set; }

        public string Recebido { get; set; }

        public string Modificado { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        public string Nome { get; set; }

        public int? Contacto { get; set; }

        public int? Idade { get; set; }

        public string OutrosDetalhesPessoa { get; set; }

        public string Descricao { get; set; }

        public ResumoPedidoViewModel(Pedido pedido)
        {
            Id = pedido.Id;
            Estado = new ResumoEstadoViewModel(pedido.EstadoAtual);
            Recebido = pedido.Tempo.ToString();
            Modificado = pedido.Modificado.ToString();
            Latitude = pedido.LocalizacaoAtual.Latitude;
            Longitude = pedido.LocalizacaoAtual.Longitude;
            if (pedido.Nome == null && pedido.Contacto == null && pedido.Idade == null && pedido.OutrosDetalhesPessoa == null)
                OutrosDetalhesPessoa = "-";
            else
            {
                Nome = pedido.Nome;
                Contacto = pedido.Contacto;
                Idade = pedido.Idade;
                OutrosDetalhesPessoa = pedido.OutrosDetalhesPessoa;
            }
            Descricao = pedido.Descricao ?? "-";
        }
    }

    public class PedidoViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Aplicação")]
        public string Aplicacao { get; set; }

        public bool AvAplicacao { get; set; }

        [DisplayName("Avaliação")]
        public string Avaliacao { get; set; }

        public ResumoEstadoViewModel Estado { get; set; }

        [DisplayName("Estado")]
        public List<ResumoEstadoViewModel> Proximos { get; set; }

        public List<AlteracoesViewModel> Alteracoes { get; set; }

        public List<MapsViewModel> Localizacoes { get; set; }

        public string Nome { get; set; }

        public int? Contacto { get; set; }

        public int? Idade { get; set; }

        [DataType(DataType.MultilineText)]
        public string OutrosDetalhesPessoa { get; set; }

        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public PedidoViewModel(Pedido pedido)
        {
            Id = pedido.Id;

            Aplicacao = pedido.Autorizacao == null ? "Aplicação não autenticada" : pedido.Autorizacao.Aplicacao.Nome;
            AvAplicacao = pedido.Avaliacao.Resultado != Resultado.ErroAutenticacao;
            switch (pedido.Avaliacao.Resultado)
            {
                case Resultado.DadosInsuficientes:
                    Avaliacao = "Sem dados suficientes para tratar.";
                    break;
                case Resultado.ErroAutenticacao:
                    Avaliacao = "A autenticação da aplicação falhou.";
                    break;
                case Resultado.MauFormato:
                    Avaliacao = "Pedido enviado com um formato errado.";
                    break;
                case Resultado.Repetido:
                    Avaliacao = "Pedido potencialmente repetido.";
                    break;
                case Resultado.Suspeito:
                    Avaliacao = "Alguns dados do pedido são potencialmente falsos. Verifique abaixo.";
                    break;
                case Resultado.Valido:
                    Avaliacao = "✔️";
                    break;
                default:
                    Avaliacao = "A avaliação falhou por motivos desconhecidos.";
                    break;
            }

            Estado = new ResumoEstadoViewModel(pedido.EstadoAtual);

            Proximos = new List<ResumoEstadoViewModel>();
            Proximos.Add(Estado);
            foreach (TransicaoDeEstado e in pedido.EstadoAtual.Seguintes)
                Proximos.Add(new ResumoEstadoViewModel(e.Para));

            // Provavelmente não é a melhor maneira de fazer isto...
            Alteracoes = new List<AlteracoesViewModel>();
            int c = 0;
            foreach (AlteracaoPedido a in pedido.Alteracoes.OrderBy(a => a.Tempo))
            {
                if (c == 0)
                {
                    c = pedido.Alteracoes.Where(b => Math.Abs((b.Tempo - a.Tempo).TotalSeconds) <= 1 && b.Utilizador == a.Utilizador).Count();
                    Alteracoes.Add(new AlteracoesViewModel(a, c));
                }
                else
                    Alteracoes.Add(new AlteracoesViewModel(a, 0));
                c--;
            }

            Localizacoes = new List<MapsViewModel>();
            foreach (Localizacao l in pedido.Localizacoes.OrderBy(l => l.Tempo))
                Localizacoes.Add(new MapsViewModel(l));

            Nome = pedido.Nome;
            Contacto = pedido.Contacto;
            Idade = pedido.Idade;
            OutrosDetalhesPessoa = pedido.OutrosDetalhesPessoa;
            Descricao = pedido.Descricao;

            if (pedido.Imagens.Count > 0)
                Imagem = "/Content/Imagens/" + pedido.Imagens.First().Url;
        }
    }

    public class PedidoEditadoViewModel
    {
        public Guid Id { get; set; }

        public int Estado { get; set; }

        public string Nome { get; set; }

        public int? Contacto { get; set; }

        public int? Idade { get; set; }

        public string OutrosDetalhes { get; set; }

        public string Descricao { get; set; }
    }

    public class MapsViewModel
    {
        public float lat { get; set; }

        public float lng { get; set; }

        public MapsViewModel(Localizacao localizacao)
        {
            lat = (float)localizacao.Latitude;
            lng = (float)localizacao.Longitude;
        }
    }

    public class AlteracoesViewModel
    {
        public int Linhas { get; set; }

        public DateTime? Tempo { get; set; }

        public string Operador { get; set; }

        public string Campo { get; set; }

        public string Valor { get; set; }

        public bool Avaliacao { get; set; }

        public AlteracoesViewModel(AlteracaoPedido alteracao, int linhas)
        {
            Linhas = linhas;
            Tempo = alteracao.Tempo;
            Operador = alteracao.Utilizador == null ? null : alteracao.Utilizador.UserName;
            Campo = alteracao.Campo.ToString();
            Valor = alteracao.Valor;
            Avaliacao = alteracao.AvaliacaoValor;
        }
    }
}