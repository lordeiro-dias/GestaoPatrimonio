using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoPatrimonio.Repositories
{
    public class LogPatrimonioRepository : ILogPatrimonioRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public LogPatrimonioRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<LogPatrimonio> Listar()
        {
            return _context.LogPatrimonio
                .Include(l => l.Usuario)
                .Include(l => l.Local)
                .Include(l => l.TipoAlteracao)
                .Include(l => l.StatusPatrimonio)
                .Include(l => l.Patrimonio)
                .OrderByDescending(l => l.DataTransferencia).ToList();
        }

        public List<LogPatrimonio> BuscarPorPatrimonio(Guid id)
        {
            return _context.LogPatrimonio
                .Include(l => l.Usuario)
                .Include(l => l.Local)
                .Include(l => l.TipoAlteracao)
                .Include(l => l.StatusPatrimonio)
                .Include(l => l.Patrimonio)
                .Where(l => l.PatrimonioID == id)
                .OrderByDescending(l => l.DataTransferencia).ToList();
        }
    }
}
