using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class ResumoEstadoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public string Imagem { get; set; }

        public bool Inicial { get; set; }

        public bool Cancelado { get; set; }
    }

    public class ListaEstadosViewModel
    {
        public List<ResumoEstadoViewModel> Estados { get; set; }

        public int Inicial { get; set; }

        public int Cancelado { get; set; }

        public ListaEstadosViewModel()
        {
            Estados = new List<ResumoEstadoViewModel>();
        }
    }

    public class EditarEstadosViewModel
    {
        public int[] Ativos { get; set; }

        public int Inicial { get; set; }

        public int Cancelado { get; set; }
    }
}