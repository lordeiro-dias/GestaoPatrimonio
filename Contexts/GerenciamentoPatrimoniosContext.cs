using System;
using System.Collections.Generic;
using GerenciamentoPatrimonio.Domains;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoPatrimonio.Contexts;

public partial class GerenciamentoPatrimoniosContext : DbContext
{
    public GerenciamentoPatrimoniosContext()
    {
    }

    public GerenciamentoPatrimoniosContext(DbContextOptions<GerenciamentoPatrimoniosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Area { get; set; }

    public virtual DbSet<Bairro> Bairro { get; set; }

    public virtual DbSet<Cargo> Cargo { get; set; }

    public virtual DbSet<Cidade> Cidade { get; set; }

    public virtual DbSet<Endereco> Endereco { get; set; }

    public virtual DbSet<Local> Local { get; set; }

    public virtual DbSet<LogPatrimonio> LogPatrimonio { get; set; }

    public virtual DbSet<Patrimonio> Patrimonio { get; set; }

    public virtual DbSet<SolicitacaoTransferencia> SolicitacaoTransferencia { get; set; }

    public virtual DbSet<StatusPatrimonio> StatusPatrimonio { get; set; }

    public virtual DbSet<StatusTransferencia> StatusTransferencia { get; set; }

    public virtual DbSet<TipoAlteracao> TipoAlteracao { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaID).HasName("PK__Area__70B8202863648A8C");

            entity.HasIndex(e => e.NomeArea, "UQ__Area__9A7797608062C629").IsUnique();

            entity.Property(e => e.AreaID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeArea)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Bairro>(entity =>
        {
            entity.HasKey(e => e.BairroID).HasName("PK__Bairro__4A093623DF330D73");

            entity.Property(e => e.BairroID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeBairro)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Cidade).WithMany(p => p.Bairro)
                .HasForeignKey(d => d.CidadeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bairro_Cidade");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.CargoID).HasName("PK__Cargo__B4E665EDF1639A48");

            entity.HasIndex(e => e.NomeCargo, "UQ__Cargo__4D9FD7DEEA67888A").IsUnique();

            entity.Property(e => e.CargoID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeCargo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.HasKey(e => e.CidadeID).HasName("PK__Cidade__B680095999893E26");

            entity.Property(e => e.CidadeID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NomeCidade)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.EnderecoID).HasName("PK__Endereco__B9D9462F3C64CB87");

            entity.Property(e => e.EnderecoID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CEP)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Complemento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Logradouro)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Bairro).WithMany(p => p.Endereco)
                .HasForeignKey(d => d.BairroID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Endereco_Bairro");
        });

        modelBuilder.Entity<Local>(entity =>
        {
            entity.HasKey(e => e.LocalID).HasName("PK__Local__499359DBE1606C39");

            entity.ToTable(tb => tb.HasTrigger("trg_Local_SoftDelete"));

            entity.Property(e => e.LocalID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.DescricaoSAP)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Local)
                .HasForeignKey(d => d.AreaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Local_Area");

            entity.HasMany(d => d.Usuario).WithMany(p => p.Local)
                .UsingEntity<Dictionary<string, object>>(
                    "LocalUsuario",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_LocalUsuario_Usuario"),
                    l => l.HasOne<Local>().WithMany()
                        .HasForeignKey("LocalID")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_LocalUsuario_Local"),
                    j =>
                    {
                        j.HasKey("LocalID", "UsuarioID");
                    });
        });

        modelBuilder.Entity<LogPatrimonio>(entity =>
        {
            entity.HasKey(e => e.LogPatrimonioID).HasName("PK__LogPatri__E716D12B05B12759");

            entity.Property(e => e.LogPatrimonioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataTransferencia).HasPrecision(0);

            entity.HasOne(d => d.Local).WithMany(p => p.LogPatrimonio)
                .HasForeignKey(d => d.LocalID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogPatrimonio_Local");

            entity.HasOne(d => d.Patrimonio).WithMany(p => p.LogPatrimonio)
                .HasForeignKey(d => d.PatrimonioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogPatrimonio_Patrimonio");

            entity.HasOne(d => d.StatusPatrimonio).WithMany(p => p.LogPatrimonio)
                .HasForeignKey(d => d.StatusPatrimonioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogPatrimonio_StatusPatrimonio");

            entity.HasOne(d => d.TipoAlteracao).WithMany(p => p.LogPatrimonio)
                .HasForeignKey(d => d.TipoAlteracaoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogPatrimonio_TipoAlteracao");

            entity.HasOne(d => d.Usuario).WithMany(p => p.LogPatrimonio)
                .HasForeignKey(d => d.UsuarioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogPatrimonio_Usuario");
        });

        modelBuilder.Entity<Patrimonio>(entity =>
        {
            entity.HasKey(e => e.PatrimonioID).HasName("PK__Patrimon__C5A60BDE13A6D296");

            entity.ToTable(tb => tb.HasTrigger("trg_Patrimonio_SoftDelete"));

            entity.Property(e => e.PatrimonioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Denominacao).IsUnicode(false);
            entity.Property(e => e.Imagem).IsUnicode(false);
            entity.Property(e => e.NumeroPatrimonio)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Local).WithMany(p => p.Patrimonio)
                .HasForeignKey(d => d.LocalID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patrimonio_Local");

            entity.HasOne(d => d.StatusPatrimonio).WithMany(p => p.Patrimonio)
                .HasForeignKey(d => d.StatusPatrimonioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patrimonio_StatusPatrimonio");
        });

        modelBuilder.Entity<SolicitacaoTransferencia>(entity =>
        {
            entity.HasKey(e => e.SolicitacaoTransferenciaID).HasName("PK__Solicita__B4E92CD2B3C26E3D");

            entity.Property(e => e.SolicitacaoTransferenciaID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DataCriacaoSolicitacao).HasPrecision(0);
            entity.Property(e => e.DataResposta).HasPrecision(0);
            entity.Property(e => e.Justificativa).IsUnicode(false);

            entity.HasOne(d => d.Local).WithMany(p => p.SolicitacaoTransferencia)
                .HasForeignKey(d => d.LocalID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SolicitacaoTransferencia_Local");

            entity.HasOne(d => d.Patrimonio).WithMany(p => p.SolicitacaoTransferencia)
                .HasForeignKey(d => d.PatrimonioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SolicitacaoTransferencia_Patrimonio");

            entity.HasOne(d => d.StatusTransferencia).WithMany(p => p.SolicitacaoTransferencia)
                .HasForeignKey(d => d.StatusTransferenciaID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SolicitacaoTransferencia_StatusTransferencia");

            entity.HasOne(d => d.UsuarioIDAprovacaoNavigation).WithMany(p => p.SolicitacaoTransferenciaUsuarioIDAprovacaoNavigation)
                .HasForeignKey(d => d.UsuarioIDAprovacao)
                .HasConstraintName("FK_SolicitacaoTransferencia_UsuarioAprovacao");

            entity.HasOne(d => d.UsuarioIDSolicitacaoNavigation).WithMany(p => p.SolicitacaoTransferenciaUsuarioIDSolicitacaoNavigation)
                .HasForeignKey(d => d.UsuarioIDSolicitacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SolicitacaoTransferencia_UsuarioSolicitacao");
        });

        modelBuilder.Entity<StatusPatrimonio>(entity =>
        {
            entity.HasKey(e => e.StatusPatrimonioID).HasName("PK__StatusPa__B3F336095794DE48");

            entity.HasIndex(e => e.Status, "UQ__StatusPa__3A15923F0357622C").IsUnique();

            entity.Property(e => e.StatusPatrimonioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StatusTransferencia>(entity =>
        {
            entity.HasKey(e => e.StatusTransferenciaID).HasName("PK__StatusTr__7AA828B9C4712928");

            entity.HasIndex(e => e.Status, "UQ__StatusTr__3A15923F067E7C4F").IsUnique();

            entity.Property(e => e.StatusTransferenciaID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoAlteracao>(entity =>
        {
            entity.HasKey(e => e.TipoAlteracaoID).HasName("PK__TipoAlte__9BEF4F0D11369AD6");

            entity.HasIndex(e => e.Tipo, "UQ__TipoAlte__8E762CB4345AA68E").IsUnique();

            entity.Property(e => e.TipoAlteracaoID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.TipoUsuarioID).HasName("PK__TipoUsua__7F22C702AD67324E");

            entity.HasIndex(e => e.NomeTipo, "UQ__TipoUsua__7859A10AA891F180").IsUnique();

            entity.Property(e => e.TipoUsuarioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.NomeTipo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioID).HasName("PK__Usuario__2B3DE79892667E5D");

            entity.ToTable(tb => tb.HasTrigger("trg_Usuario_SoftDelete"));

            entity.HasIndex(e => e.RG, "UQ__Usuario__321537C8B5D79D23").IsUnique();

            entity.HasIndex(e => e.CarteiraTrabalho, "UQ__Usuario__6E25BCA201B55A89").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534803D9DC1").IsUnique();

            entity.HasIndex(e => e.CPF, "UQ__Usuario__C1F897318861ED58").IsUnique();

            entity.Property(e => e.UsuarioID).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.CPF)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.CarteiraTrabalho)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NIF)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PrimeiroAcesso).HasDefaultValue(true);
            entity.Property(e => e.RG)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(32);

            entity.HasOne(d => d.Cargo).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.CargoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Cargo");

            entity.HasOne(d => d.Endereco).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.EnderecoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Endereco");

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.TipoUsuarioID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_TipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
