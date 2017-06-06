using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ESPM.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Aplicacao Aplicacao { get; set; }

        public virtual List<Descricao> Descricoes { get; set; }

        public virtual List<EstadoDePedido> Estados { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Definicao> Definicoes { get; set; }
        public DbSet<MudancaDefinicao> MudancasDefinicoes { get; set; }
        public DbSet<Aplicacao> Aplicacoes { get; set; }
        public DbSet<Autorizacao> Autorizacoes { get; set; }
        public DbSet<Descricao> Descricoes { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<EstadoDePedido> EstadosDePedido { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<TransicaoDeEstado> TransicoesDeEstado { get; set; }
    }
}