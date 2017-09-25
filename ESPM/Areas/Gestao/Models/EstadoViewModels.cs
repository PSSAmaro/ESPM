using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESPM.Areas.Gestao.Models
{
    public class EstadoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public int Icone { get; set; }

        public bool Inicial { get; set; }

        public bool Cancelado { get; set; }

        public int Familias { get; set; }

        public List<ResumoEstadoViewModel> Anteriores { get; set; }

        public List<ResumoEstadoViewModel> Seguintes { get; set; }

        public EstadoViewModel()
        {
            Anteriores = new List<ResumoEstadoViewModel>();
            Seguintes = new List<ResumoEstadoViewModel>();
        }
    }

    public class ResumoEstadoViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public bool Ativo { get; set; }

        public string Icone { get; set; }
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

    public class NovoEstadoViewModel
    {
        [Required]
        public string Nome { get; set; }
    }

    public class EditarEstadosViewModel
    {
        public int[] Ativos { get; set; }

        [Required]
        public int Inicial { get; set; }

        [Required]
        public int Cancelado { get; set; }
    }
}