using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    /// <summary>
    /// Definição do sistema.
    /// </summary>
    // Talvez haja uma forma nativa de fazer?
    // Serão demasiados campos? Better safe than sorry
    // Valores iniciais em DbInitializer
    [Table("Definicoes")]
    public class Definicao
    {
        /// <summary>
        /// Nome da definição.
        /// </summary>
        [Key]
        public string Nome { get; set; }

        /// <summary>
        /// Valor máximo da definição.
        /// </summary>
        // Se não houver máximo, Maximo = 0
        [Required]
        [DisplayName("Máximo")]
        public int Maximo { get; set; }

        /// <summary>
        /// Valor atual da definição.
        /// </summary>
        [Required]
        [DisplayName("Estado")]
        public int Valor { get; set; }

        /// <summary>
        /// Nome de apresentação da definição.
        /// </summary>
        [Required]
        [DisplayName("Definição")]
        public string Apresentacao { get; set; }

        /// <summary>
        /// Descrição da definição.
        /// </summary>
        [Required]
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        /// <summary>
        /// O que significa esta definição ser 0?
        /// </summary>
        [Required]
        public string SignificadoZero { get; set; }

        /// <summary>
        /// O que significa esta definição ser diferente de 0?
        /// </summary>
        [Required]
        public string SignificadoOutro { get; set; }

        /// <summary>
        /// Indica se esta definição deve estar na lista de definições
        /// </summary>
        [Required]
        public bool Listar { get; set; }

        /// <summary>
        /// Lista de modificações a esta definição.
        /// </summary>
        public virtual List<AlteracaoDefinicao> Alteracoes { get; set; }

        /// <summary>
        /// Inicializa as listas para evitar NullReferenceException.
        /// </summary>
        public Definicao()
        {
            Alteracoes = new List<AlteracaoDefinicao>();
        }

        // Lógica nos modelos devia ser evitado, mas as definições vão ter de ser especiais
        // Devolve o novo valor, -1 se não for alterado, -2 se houver erro
        public int Alterar(int v, ApplicationUser u)
        {
            // Verificar se o valor é diferente
            if (Valor != v)
            {
                // Verificar se o valor é válido
                if (v >= 0 && (Maximo == 0 || v <= Maximo))
                {
                    Alteracoes.Add(new AlteracaoDefinicao()
                    {
                        Utilizador = u,
                        De = Valor,
                        Para = v,
                        Tempo = DateTime.Now
                    });
                    Valor = v;
                    return Valor;
                }
                else
                    return -2;
            }
            return -1;
        }
    }
}