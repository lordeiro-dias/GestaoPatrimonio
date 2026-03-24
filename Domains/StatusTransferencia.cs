using System;
using System.Collections.Generic;

namespace GerenciamentoPatrimonio.Domains;

public partial class StatusTransferencia
{
    public Guid StatusTransferenciaID { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<SolicitacaoTransferencia> SolicitacaoTransferencia { get; set; } = new List<SolicitacaoTransferencia>();
}
