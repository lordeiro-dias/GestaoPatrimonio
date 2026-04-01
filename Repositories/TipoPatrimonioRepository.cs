using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class TipoPatrimonioRepository : ITipoPatrimonioRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public TipoPatrimonioRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoPatrimonio> Listar()
        {
            return _context.TipoPatrimonio.OrderBy(t => t.NomeTipo).ToList();
        }

        public TipoPatrimonio BuscarPorId(Guid id)
        {
            return _context.TipoPatrimonio.Find(id);
        }

        public TipoPatrimonio BuscarPorNome(string nome)
        {
            return _context.TipoPatrimonio.FirstOrDefault(t => t.NomeTipo == nome);
        }

        public void Adicionar(TipoPatrimonio tipo)
        {
            _context.TipoPatrimonio.Add(tipo);
            _context.SaveChanges();
        }

        public void Atualizar(TipoPatrimonio tipo)
        {
            if (tipo == null)
            {
                return;
            }

            TipoPatrimonio tipoBanco = _context.TipoPatrimonio.Find(tipo.TipoPatrimonioID);

            if (tipoBanco == null)
            {
                return;
            }

            tipoBanco.NomeTipo = tipo.NomeTipo;

            _context.SaveChanges();
        }
    }
}
