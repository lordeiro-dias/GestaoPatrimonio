using System;
using System.Collections.Generic;

namespace GerenciamentoPatrimonio.Domains;

public partial class TipoAlteracao
{
    public Guid TipoAlteracaoID { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<LogPatrimonio> LogPatrimonio { get; set; } = new List<LogPatrimonio>();
}
