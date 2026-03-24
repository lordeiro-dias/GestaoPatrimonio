using System;
using System.Collections.Generic;

namespace GerenciamentoPatrimonio.Domains;

public partial class SolicitacaoTransferencia
{
    public Guid SolicitacaoTransferenciaID { get; set; }

    public DateTime DataCriacaoSolicitacao { get; set; }

    public DateTime? DataResposta { get; set; }

    public string Justificativa { get; set; } = null!;

    public Guid StatusTransferenciaID { get; set; }

    public Guid UsuarioIDSolicitacao { get; set; }

    public Guid? UsuarioIDAprovacao { get; set; }

    public Guid PatrimonioID { get; set; }

    public Guid LocalID { get; set; }

    public virtual Local Local { get; set; } = null!;

    public virtual Patrimonio Patrimonio { get; set; } = null!;

    public virtual StatusTransferencia StatusTransferencia { get; set; } = null!;

    public virtual Usuario? UsuarioIDAprovacaoNavigation { get; set; }

    public virtual Usuario UsuarioIDSolicitacaoNavigation { get; set; } = null!;
}
