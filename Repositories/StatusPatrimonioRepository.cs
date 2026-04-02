using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class StatusPatrimonioRepository : IStatusPatrimonioRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public StatusPatrimonioRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<StatusPatrimonio> Listar()
        {
            return _context.StatusPatrimonio.OrderBy(s => s.Status).ToList();
        }

        public StatusPatrimonio BuscarPorId(Guid id)
        {
            return _context.StatusPatrimonio.Find(id);
        }

        public StatusPatrimonio BuscarPorNome(string nomeStatus)
        {
            return _context.StatusPatrimonio.FirstOrDefault(s => s.Status.ToLower() == nomeStatus.ToLower());
        }

        public void Adicionar(StatusPatrimonio statusPatrimonio)
        {
            _context.StatusPatrimonio.Add(statusPatrimonio);
            _context.SaveChanges();
        }

        public void Atualizar(StatusPatrimonio statusPatrimonio)
        {
            if(statusPatrimonio == null)
            {
                return;
            }

            StatusPatrimonio statusBanco = _context.StatusPatrimonio.Find(statusPatrimonio.StatusPatrimonioID);

            if(statusBanco == null)
            {
                return;
            }

            statusBanco.Status = statusPatrimonio.Status;
            _context.SaveChanges();
        }
    }
}
