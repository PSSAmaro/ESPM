using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class ResumoPedidoViewModel
    {
        public Guid Id { get; set; }

        public ResumoEstadoViewModel Estado { get; set; }

        public DateTime Recebido { get; set; }

        public DateTime Modificado { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        public string Nome { get; set; }

        public int? Contacto { get; set; }
    }

    public class PedidoViewModel
    {
        public Guid Id { get; set; }

        public ResumoEstadoViewModel Estado { get; set; }

        public List<ResumoEstadoViewModel> Proximos { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }

        public string Nome { get; set; }

        public int? Contacto { get; set; }

        public string Descricao { get; set; }
    }
}