using System;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.BdContextEvent;

/// <summary>
/// Classe de Contexto do Entity Framework Core (Database First).
/// 
/// O QUE É O DBCONTEXT?
/// O DbContext é a ponte de comunicação principal entre a aplicação em C# e o Banco de Dados SQL Server.
/// Ele mapeia as tabelas do banco em propriedades do tipo DbSet e gerencia a criação, consulta, 
/// alteração e remoção de registros via ORM (Object-Relational Mapping).
/// </summary>
public partial class EventContext : DbContext
{
    /// <summary>
    /// Construtor padrão sem parâmetros.
    /// </summary>
    public EventContext()
    {
    }

    /// <summary>
    /// Construtor que recebe as opções de configuração do DbContext (ex: String de Conexão).
    /// </summary>
    /// <param name="options">Opções de configuração injetadas pelo container de Injeção de Dependências.</param>
    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Tabela de Comentários recebidos nos eventos.
    /// </summary>
    public virtual DbSet<Comentario> Comentario { get; set; }

    /// <summary>
    /// Tabela de Eventos cadastrados.
    /// </summary>
    public virtual DbSet<Evento> Evento { get; set; }

    /// <summary>
    /// Tabela de Instituições promotoras de eventos.
    /// </summary>
    public virtual DbSet<Instituicao> Instituicao { get; set; }

    /// <summary>
    /// Tabela de confirmações de Presença dos usuários.
    /// </summary>
    public virtual DbSet<Presenca> Presenca { get; set; }

    /// <summary>
    /// Tabela de Tipos/Categorias de Eventos.
    /// </summary>
    public virtual DbSet<TipoEvento> TipoEvento { get; set; }

    /// <summary>
    /// Tabela de Perfis/Tipos de Usuários.
    /// </summary>
    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    /// <summary>
    /// Tabela de Usuários do sistema.
    /// </summary>
    public virtual DbSet<Usuario> Usuario { get; set; }

    /// <summary>
    /// Configuração do Mapeamento de Entidades via Fluent API.
    /// 
    /// CONCEITO PARA ALUNOS:
    /// O OnModelCreating define as regras detalhadas de relacionamento, chaves primárias, chaves estrangeiras, 
    /// índices únicos e comportamentos de exclusão (Cascade Delete vs SetNull) exatamente como especificado 
    /// no Diagrama de Classes UML e no Modelo Relacional (DER).
    /// </summary>
    /// <param name="modelBuilder">Construtor do modelo do Entity Framework.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Mapeamento da Entidade TipoUsuario
        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario);
            entity.Property(e => e.IdTipoUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo).HasMaxLength(100).IsRequired();
        });

        // 2. Mapeamento da Entidade Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.IdUsuario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Senha).HasMaxLength(60).IsRequired();

            // Relacionamento TipoUsuario -> Usuario (Associação: FK Anulável, SetNull ao apagar pai)
            entity.HasOne(d => d.IdTipoUsuarioNavigation)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 3. Mapeamento da Entidade Instituicao
        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.IdInstituicao);
            entity.Property(e => e.IdInstituicao).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CNPJ).HasMaxLength(14).IsRequired();
            entity.Property(e => e.NomeFantasia).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Endereco).HasMaxLength(100).IsRequired();
        });

        // 4. Mapeamento da Entidade TipoEvento
        modelBuilder.Entity<TipoEvento>(entity =>
        {
            entity.HasKey(e => e.IdTipoEvento);
            entity.Property(e => e.IdTipoEvento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Titulo).HasMaxLength(100).IsRequired();
        });

        // 5. Mapeamento da Entidade Evento
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento);
            entity.Property(e => e.IdEvento).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descricao).HasColumnType("text").IsRequired();
            entity.Property(e => e.DataEvento).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.ImagemUrl).HasMaxLength(2048);

            // Relacionamento TipoEvento -> Evento (Associação: FK Anulável)
            entity.HasOne(d => d.IdTipoEventoNavigation)
                .WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdTipoEvento)
                .OnDelete(DeleteBehavior.SetNull);

            // Relacionamento Instituicao -> Evento (Associação: FK Anulável)
            entity.HasOne(d => d.IdInstituicaoNavigation)
                .WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdInstituicao)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 6. Mapeamento da Entidade Presenca
        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.HasKey(e => e.IdPresenca);
            entity.Property(e => e.IdPresenca).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Situacao).IsRequired();

            // Relacionamento Evento -> Presenca (Composição: FK Obrigatória, Exclusão em Cascata)
            entity.HasOne(d => d.IdEventoNavigation)
                .WithMany(p => p.Presencas)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Usuario -> Presenca (Associação: FK Anulável, SetNull ao apagar Usuário)
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Presencas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 7. Mapeamento da Entidade Comentario
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario);
            entity.Property(e => e.IdComentario).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Descricao).HasMaxLength(200).IsRequired();
            entity.Property(e => e.DataComentario).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.Exibe)
            .IsRequired()
            .ValueGeneratedNever();

            // Relacionamento Evento -> Comentario (Composição: FK Obrigatória, Exclusão em Cascata)
            entity.HasOne(d => d.IdEventoNavigation)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento Usuario -> Comentario (Associação: FK Anulável, SetNull ao apagar Usuário)
            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
