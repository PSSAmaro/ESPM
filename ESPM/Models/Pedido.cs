﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ESPM.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        // Autorização utilizada para o pedido
        // É utilizada a autorização em vez da aplicação para ser mais fácil distinguir pedidos válidos, pedidos inválidos e pedidos de teste
        [Required]
        public virtual Autorizacao Autorizacao { get; set; }

        // Momento em que o pedido foi enviado
        // Se não houver informação de tempo no pedido é o momento em que é recebido
        // Falta definir o valor predefinido como a hora atual
        public DateTime Tempo { get; set; }

        // Nome da pessoa que precisa de ajuda
        public string Nome { get; set; }

        // Contacto da pessoa que precisa de ajuda
        public int? Contacto { get; set; }

        // Idade da pessoa que precisa de ajuda
        public int? Idade { get; set; }

        // Outras informações relevantes sobre a pessoa que precisa de ajuda
        public string OutrosDetalhesPessoa { get; set; }

        // Histórico de descrições do pedido de ajuda
        // Descrição atual: Descricoes.OrderByDescending(d => d.Tempo).FirstOrDefault()
        public virtual List<Descricao> Descricoes { get; set; }

        // Lista de localizações recebidas
        public virtual List<Localizacao> Localizacoes { get; set; }

        // Lista de imagens recebidas
        public virtual List<Imagem> Imagens { get; set; }

        // Lista de estados pelos quais o pedido já passou
        // Estado atual: Estados.OrderByDescending(e => e.Tempo).FirstOrDefault()
        // Aberto: EstadoAtual.Estado.EstadoFinal()
        public virtual List<EstadoDePedido> Estados { get; set; }

        public Pedido()
        {
            Descricoes = new List<Descricao>();
            Localizacoes = new List<Localizacao>();
            Imagens = new List<Imagem>();
            Estados = new List<EstadoDePedido>();
        }
    }
}