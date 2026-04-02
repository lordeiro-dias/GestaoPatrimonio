using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class StatusTransferenciaRepository : IStatusTransferenciaRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public StatusTransferenciaRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<StatusTransferencia> Listar()
        {
            return _context.StatusTransferencia.OrderBy(s => s.Status).ToList();
        }

        public StatusTransferencia BuscarPorId(Guid id)
        {
            return _context.StatusTransferencia.Find(id);
        }

        public StatusTransferencia BuscarPorNome(string nomeStatus)
        {
            return _context.StatusTransferencia.FirstOrDefault(s => s.Status.ToLower() == nomeStatus.ToLower());
        }

        public void Adicionar(StatusTransferencia statusTransferencia)
        {
            _context.StatusTransferencia.Add(statusTransferencia);
            _context.SaveChanges();
        }

        public void Atualizar(StatusTransferencia statusTransferencia)
        {
            if(statusTransferencia == null)
            {
                return;
            }

            StatusTransferencia statusBanco = _context.StatusTransferencia.Find(statusTransferencia.StatusTransferenciaID);

            if(statusBanco == null)
            {
                return;
            }

            statusBanco.Status = statusTransferencia.Status;

            _context.SaveChanges();
        }
    }
}
