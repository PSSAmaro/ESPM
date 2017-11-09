using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Imagem de um pedido.
    /// </summary>
    [Table("Imagens")]
    public class Imagem
    {
        /// <summary>
        /// ID.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Pedido a que pertence esta imagem.
        /// </summary>
        [Required]
        public virtual Pedido Pedido { get; set; }

        /// <summary>
        /// Momento em que esta fotografia foi tirada.
        /// </summary>
        public DateTime Tempo { get; private set; }

        /// <summary>
        /// URL da imagem.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Descrição da imagem
        /// </summary>
        public string Descricao { get; private set; }

        public Imagem(ImagemViewModel imagem)
        {
            // Se calhar arranjar um campo público que devolva diretamente a imagem (e se calhar guardar não devia ser feito aqui, mas...)

            Tempo = imagem.Tempo ?? DateTime.Now;
            // É PRECISO MUDAR A PRÓXIMA LINHA PARA GARANTIR QUE NÃO HÁ NOMES REPETIDOS E CONFIRMAR O TIPO
            Url = Tempo.Ticks.ToString() + ".jpg";
            Descricao = imagem.Descricao;

            // Guardar a imagem
            // FALTA CONFIRMAR A EXTENSÃO
            string caminho = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Imagens"), Url);
            int inicio = imagem.Dados.IndexOf(",");
            string sub;
            if (inicio == -1)
                sub = imagem.Dados;
            else
                sub = imagem.Dados.Substring(inicio + 1);
            File.WriteAllBytes(caminho, Convert.FromBase64String(sub));
        }

        public Imagem() { }
    }
}