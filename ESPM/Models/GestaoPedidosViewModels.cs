using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    public class PedidoViewModel
    {
        public Guid Id { get; set; }

        public DateTime Tempo { get; set; }

        public PedidoViewModel(Pedido pedido)
        {
            Id = pedido.Id;
            Tempo = pedido.Tempo;
        }
    }

    public class ResumoPedidoViewModel
    {
        public Guid Id { get; set; }

        public DateTime Tempo { get; set; }

        // Só talvez
        public string Descricao { get; set; }

        public EstadoViewModel Estado { get; set; }

        public List<EstadoViewModel> Seguintes { get; set; }

        public ResumoPedidoViewModel(Pedido pedido)
        {
            Id = pedido.Id;
            Tempo = pedido.Tempo;
            if (pedido.Descricoes.Count > 0)
                Descricao = pedido.Descricoes.OrderByDescending(d => d.Tempo).FirstOrDefault().Texto;
            Estado atual = pedido.Estados.OrderByDescending(e => e.Tempo).FirstOrDefault().Estado;
            Estado = new EstadoViewModel(atual);
            if (atual.Seguintes.Count > 0)
                Seguintes = atual.Seguintes.Select(t => new EstadoViewModel(t.Para)).ToList();
            else
                Seguintes = new List<EstadoViewModel>();
        }
    }

    public class EstadoViewModel
    {
        public int Id { get; set; }

        public int Familia { get; set; }

        public string Nome { get; set; }

        public EstadoViewModel(Estado estado)
        {
            Id = estado.Id;
            Familia = estado.Familia;
            Nome = estado.Nome;
        }
    }

    public class AlterarEstadoViewModel
    {
        public string Nome { get; set; }
    }
}