using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.SolicitacaoTransferenciaDto;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class SolicitacaoTransferenciaRepository : ISolicitacaoTransferenciaRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public SolicitacaoTransferenciaRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<SolicitacaoTransferencia> Listar()
        {
            return _context.SolicitacaoTransferencia.OrderByDescending(s => s.DataCriacaoSolicitacao).ToList();
        }

        public SolicitacaoTransferencia BuscarPorId(Guid id)
        {
            return _context.SolicitacaoTransferencia.Find(id);
        }

        public StatusTransferencia BuscarStatusTransferenciaPorNome(string nomeStatus)
        {
            return _context.StatusTransferencia.FirstOrDefault(s => s.Status.ToLower() == nomeStatus.ToLower());
        }

        public bool ExisteSolicitacaoPendente(Guid patrimonioId)
        {
            StatusTransferencia statusPendente = BuscarStatusTransferenciaPorNome("Pendente de aprovação");

            if(statusPendente == null)
            {
                return false;
            }

            return _context.SolicitacaoTransferencia.Any(solicitacao => solicitacao.PatrimonioID == patrimonioId 
            && solicitacao.StatusTransferenciaID == statusPendente.StatusTransferenciaID);
        }

        public bool UsuarioResponsavelDaLocalizacao(Guid usuarioId, Guid localId)
        {
            return _context.Usuario.Any(u => u.UsuarioID == usuarioId && u.Local.Any(l => l.LocalID == localId));
        }

        public void Adicionar(SolicitacaoTransferencia solicitacaoTransferencia)
        {
            _context.SolicitacaoTransferencia.Add(solicitacaoTransferencia);
            _context.SaveChanges();
        }

        public bool LocalExiste(Guid localId)
        {
            return _context.Local.Any(l => l.LocalID == localId);
        }

        public Patrimonio BuscarPatrimonioPorId(Guid patrimonioId)
        {
            return _context.Patrimonio.Find(patrimonioId);
        }
    }
}
