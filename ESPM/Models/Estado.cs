﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Estados")]
    public class Estado
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int Familia { get; set; }

        [Required]
        public bool Ativo { get; set; }

        // Lista de estados dos quais é possível chegar a este estado
        // Nome do estado anterior: Anteriores.De.Nome
        [InverseProperty("Para")]
        public virtual List<TransicaoDeEstado> Anteriores { get; set; }

        // Lista de estados para os quais este pode transitar
        // Nome do estado seguinte: Seguintes.Para.Nome
        [InverseProperty("De")]
        public virtual List<TransicaoDeEstado> Seguintes { get; set; }

        // Este é o estado inicial se não tiver estados anteriores (Só deve haver 1)
        // Apagar se não for possível usar
        public bool EstadoInicial()
        {
            return Anteriores.Count == 0;
        }

        // Este é um estado final se não tiver estados seguintes
        public bool EstadoFinal()
        {
            return Seguintes.Count == 0;
        }
    }
}