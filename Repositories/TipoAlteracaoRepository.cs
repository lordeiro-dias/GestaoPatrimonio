using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class TipoAlteracaoRepository : ITipoAlteracaoRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public TipoAlteracaoRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoAlteracao> Listar()
        {
            return _context.TipoAlteracao.OrderBy(t => t.Tipo).ToList();
        }

        public TipoAlteracao BuscarPorId(Guid id)
        {
            return _context.TipoAlteracao.Find(id);
        }

        public TipoAlteracao BuscarPorNome(string nome)
        {
            return _context.TipoAlteracao.FirstOrDefault(t => t.Tipo == nome);
        }

        public void Adicionar(TipoAlteracao tipoAlteracao)
        {
            _context.TipoAlteracao.Add(tipoAlteracao);
            _context.SaveChanges();
        }

        public void Atualizar(TipoAlteracao tipoAlteracao)
        {
            if(tipoAlteracao == null)
            {
                return;
            }

            TipoAlteracao tipoBanco = _context.TipoAlteracao.Find(tipoAlteracao.TipoAlteracaoID);

            if(tipoBanco == null)
            {
                return;
            }

            tipoBanco.Tipo = tipoAlteracao.Tipo;

            _context.SaveChanges();
        }
    }
}
