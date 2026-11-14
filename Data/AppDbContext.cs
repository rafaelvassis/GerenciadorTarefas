
using GerenciadorTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorTarefas.Data
{
    /// <summary>
    /// Ponto de acesso ao banco pelo Entity Framework Core. Define as tabelas
    /// e como as classes de <c>Models</c> são mapeadas para o SQL Server.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Recebe as opções configuradas no <c>Program.cs</c> (provedor SQL Server e connection string).
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>Tabela <c>Tarefas</c>.</summary>
        public DbSet<Tarefa> Tarefas { get; set; }

        /// <summary>Tabela <c>Categorias</c>.</summary>
        public DbSet<Categoria> Categorias { get; set; }

        /// <summary>Tabela <c>Participantes</c>.</summary>
        public DbSet<Participante> Participantes { get; set; }

        /// <summary>
        /// Configura o mapeamento: chaves, tamanhos, valores padrão, a constraint de status,
        /// os relacionamentos (1:N com categorias, N:N com participantes) e os dados iniciais.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarefa>(tarefa =>
            {
                tarefa.ToTable("Tarefas", t =>
                    t.HasCheckConstraint("CK_Tarefas_Status", "[Status] IN (0, 1, 2)"));

                tarefa.HasKey(t => t.IdTarefa);

                tarefa.Property(t => t.IdTarefa)
                    .HasDefaultValueSql("NEWID()");

                tarefa.Property(t => t.Titulo)
                    .HasMaxLength(200)
                    .IsRequired();

                tarefa.Property(t => t.Descricao)
                    .HasMaxLength(1000);

                tarefa.Property(t => t.DataCriacao)
                    .HasDefaultValueSql("SYSDATETIME()");

                tarefa.Property(t => t.Status)
                    .HasDefaultValue(EStatus.Pendente);

                tarefa.HasOne(t => t.Categoria)
                    .WithMany(c => c.Tarefas)
                    .HasForeignKey(t => t.FK_IdCategoria)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relação muitos-para-muitos: a tabela TarefasParticipantes liga tarefas e participantes.
                // Ao excluir uma tarefa ou um participante, os vínculos dele são removidos.
                tarefa.HasMany(t => t.Participantes)
                    .WithMany(p => p.Tarefas)
                    .UsingEntity(
                        "TarefasParticipantes",
                        participante => participante.HasOne(typeof(Participante)).WithMany()
                            .HasForeignKey("FK_IdParticipante").OnDelete(DeleteBehavior.Cascade),
                        t => t.HasOne(typeof(Tarefa)).WithMany()
                            .HasForeignKey("FK_IdTarefa").OnDelete(DeleteBehavior.Cascade),
                        vinculo => vinculo.HasKey("FK_IdTarefa", "FK_IdParticipante"));
            });

            modelBuilder.Entity<Categoria>(categoria =>
            {
                categoria.ToTable("Categorias");

                categoria.HasKey(c => c.IdCategoria);

                categoria.Property(c => c.IdCategoria)
                    .HasDefaultValueSql("NEWID()");

                categoria.Property(c => c.Nome)
                    .HasMaxLength(150)
                    .IsRequired();

                categoria.HasIndex(c => c.Nome).IsUnique();

                // Categorias iniciais
                categoria.HasData(
                    new Categoria { IdCategoria = Guid.Parse("8f1d6a2e-1c3b-4a5d-9e7f-000000000001"), Nome = "Geral" },
                    new Categoria { IdCategoria = Guid.Parse("8f1d6a2e-1c3b-4a5d-9e7f-000000000002"), Nome = "Trabalho" },
                    new Categoria { IdCategoria = Guid.Parse("8f1d6a2e-1c3b-4a5d-9e7f-000000000003"), Nome = "Estudos" },
                    new Categoria { IdCategoria = Guid.Parse("8f1d6a2e-1c3b-4a5d-9e7f-000000000004"), Nome = "Pessoal" });
            });

            modelBuilder.Entity<Participante>(participante =>
            {
                participante.ToTable("Participantes");

                participante.HasKey(p => p.IdParticipante);

                participante.Property(p => p.IdParticipante)
                    .HasDefaultValueSql("NEWID()");

                participante.Property(p => p.Nome)
                    .HasMaxLength(150)
                    .IsRequired();

                participante.Property(p => p.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .IsRequired();

                participante.HasIndex(p => p.Email).IsUnique();

                // Participantes iniciais
                participante.HasData(
                    new Participante { IdParticipante = Guid.Parse("3b7c9e4a-5d2f-4e8b-a1c6-000000000001"), Nome = "Ana Souza", Email = "ana@exemplo.com" },
                    new Participante { IdParticipante = Guid.Parse("3b7c9e4a-5d2f-4e8b-a1c6-000000000002"), Nome = "Bruno Lima", Email = "bruno@exemplo.com" });
            });
        }
    }
}
