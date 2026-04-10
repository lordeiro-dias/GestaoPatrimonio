using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class PatrimonioRepository : IPatrimonioRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public PatrimonioRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Patrimonio> Listar()
        {
            return _context.Patrimonio.OrderBy(p => p.Denominacao).ToList();
        }

        public Patrimonio BuscarPorId(Guid id)
        {
            return _context.Patrimonio.Find(id);
        }

        public bool BuscarPorNumeroPatrimonio(string numeroPatrimonio)
        {
            return _context.Patrimonio.Any(patrimonio => patrimonio.NumeroPatrimonio == numeroPatrimonio);
        }

        public bool LocalizacaoExiste(Guid localId)
        {
            return _context.Local.Any(l => l.LocalID == localId);
        }   

        public bool StatusPatrimonioExiste(Guid statusPatrimonioId)
        {
            return _context.StatusPatrimonio.Any(s => s.StatusPatrimonioID == statusPatrimonioId);
        }

        public Local BuscarLocalPorNome(string nomeLocal)
        {
            return _context.Local.FirstOrDefault(l => l.Nome.ToLower() == nomeLocal.ToLower());
        }

        public StatusPatrimonio BuscarStatusPatrimonioPorNome(string nomeStatus)
        {
            return _context.StatusPatrimonio.FirstOrDefault(s => s.Status.ToLower() == nomeStatus.ToLower());
        }

        public TipoAlteracao BuscarTipoAlteracaoPorNome(string nomeTipo)
        {
            return _context.TipoAlteracao.FirstOrDefault(t => t.Tipo.ToLower() == nomeTipo.ToLower());
        }

        public void Adicionar(Patrimonio patrimonio)
        {
            _context.Patrimonio.Add(patrimonio);
            _context.SaveChanges();
        }

        public void AtualizarStatus(Patrimonio patrimonio)
        {
            if (patrimonio == null)
            {
                return;
            }

            Patrimonio patrimonioBanco = _context.Patrimonio.Find(patrimonio.PatrimonioID);

            if (patrimonioBanco == null)
            {
                return;
            }

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
