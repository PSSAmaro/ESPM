using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ESPM.Models
{
    /// <summary>
    /// Utilizador da plataforma.
    /// </summary>
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Lista de aplicações pelas quais este utilizador é responsável. (Role = Aplicacao)
        /// </summary>
        public virtual List<Aplicacao> Aplicacoes { get; set; }

        /// <summary>
        /// Criar identidade.
        /// </summary>
        /// <param name="manager">Gestor de utilizadores.</param>
        /// <returns>Identidade de utilizador.</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// Contexto da BD.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Definições do sistema.
        /// </summary>
        public DbSet<Definicao> Definicoes { get; set; }

        /// <summary>
        /// Alterações às definições do sistema.
        /// </summary>
        public DbSet<AlteracaoDefinicao> AlteracoesDefinicoes { get; set; }

        /// <summary>
        /// Aplicações autorizadas.
        /// </summary>
        public DbSet<Aplicacao> Aplicacoes { get; set; }

        /// <summary>
        /// Autorizações de aplicações.
        /// </summary>
        public DbSet<Autorizacao> Autorizacoes { get; set; }

        /// <summary>
        /// Avaliações dos Requests.
        /// </summary>
        public DbSet<Avaliacao> Avaliacoes { get; set; }

        /// <summary>
        /// Descrições de pedidos.
        /// </summary>
        public DbSet<Descricao> Descricoes { get; set; }

        /// <summary>
        /// Endereços IP que enviaram pedidos.
        /// </summary>
        public DbSet<Endereco> Enderecos { get; set; }

        /// <summary>
        /// Estados possíveis para os pedidos.
        /// </summary>
        public DbSet<Estado> Estados { get; set; }

        /// <summary>
        /// Estados dos pedidos.
        /// </summary>
        public DbSet<EstadoDePedido> EstadosDePedido { get; set; }

        /// <summary>
        /// Imagens dos pedidos.
        /// </summary>
        public DbSet<Imagem> Imagens { get; set; }

        /// <summary>
        /// Localizações dos pedidos.
        /// </summary>
        public DbSet<Localizacao> Localizacoes { get; set; }

        /// <summary>
        /// Pedidos de ajuda recebidos.
        /// </summary>
        public DbSet<Pedido> Pedidos { get; set; }

        /// <summary>
        /// Transições possíveis entre estados de pedidos.
        /// </summary>
        public DbSet<TransicaoDeEstado> TransicoesDeEstado { get; set; }

        /// <summary>
        /// Ligação à BD.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Croar contexto.
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Definições aquando da criação do modelo.
        /// </summary>
        /// <param name="modelBuilder">Construtor do modelo.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}