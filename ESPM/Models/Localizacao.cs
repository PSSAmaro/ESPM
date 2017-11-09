using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Localização de um pedido.
    /// </summary>
    // Falta o erro!!!
    [Table("Localizacoes")]
    public class Localizacao
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Pedido a que se refere esta localização.
        /// </summary>
        [Required]
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Momento em que esta localização foi detetada.
        /// </summary>
        // Se não houver informação de tempo é o momento em que a informação foi recebida
        public DateTime Tempo { get; private set; }

        /// <summary>
        /// Latitude detetada.
        /// </summary>
        [Required]
        public float Latitude { get; private set; }

        /// <summary>
        /// Longitude detetada.
        /// </summary>
        [Required]
        public float Longitude { get; private set; }

        /// <summary>
        /// Erro em metros. Atualmente ignorado.
        /// </summary>
        public int? Erro { get; private set; }

        public Localizacao(LocalizacaoViewModel localizacao)
        {
            Tempo = localizacao.Tempo ?? DateTime.Now;
            Latitude = localizacao.Latitude;
            Longitude = localizacao.Longitude;
        }

        public Localizacao() { }
    }
}