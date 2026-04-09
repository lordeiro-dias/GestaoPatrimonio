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

        public StatusPatrimonio BuscarStatusPatrimonioPorNome(string nomeStatus)
        {
            return _context.StatusPatrimonio.FirstOrDefault(s => s.Status.ToLower() == nomeStatus.ToLower());
        }

        public TipoAlteracao BuscarTipoAlteracaoPorNome(string nomeTipo)
        {
            return _context.TipoAlteracao.FirstOrDefault(t => t.Tipo.ToLower() == nomeTipo.ToLower());
        }

        public void Atualizar(SolicitacaoTransferencia solicitacaoTransferencia)
        {
            if(solicitacaoTransferencia == null)
            {
                return;
            }

            SolicitacaoTransferencia solicitacaoBanco = _context.SolicitacaoTransferencia.Find(solicitacaoTransferencia.SolicitacaoTransferenciaID);
            
            if(solicitacaoBanco == null)
            {
                return;
            }

            solicitacaoBanco.DataResposta = solicitacaoTransferencia.DataResposta;
            solicitacaoBanco.StatusTransferenciaID = solicitacaoTransferencia.StatusTransferenciaID;
            solicitacaoBanco.UsuarioIDAprovacao = solicitacaoTransferencia.UsuarioIDAprovacao;

            _context.SaveChanges();
        }

        public void AtualizarPatrimonio(Patrimonio patrimonio)
        {
            if(patrimonio == null)
            {
                return;
            }

            Patrimonio patrimonioBanco = _context.Patrimonio.Find(patrimonio.PatrimonioID);

            if(patrimonioBanco == null)
            {
                return;
            }

            patrimonioBanco.LocalID = patrimonio.LocalID;
            patrimonioBanco.StatusPatrimonioID = patrimonio.StatusPatrimonioID;

            _context.SaveChanges();
        }

        public void AdicionarLog(LogPatrimonio logPatrimonio)
        {
            _context.LogPatrimonio.Add(logPatrimonio);
            _context.SaveChanges();
        }
    }
}
