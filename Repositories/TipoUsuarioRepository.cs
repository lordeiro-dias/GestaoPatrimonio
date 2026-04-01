using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public TipoUsuarioRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<TipoUsuario> Listar()
        {
            return _context.TipoUsuario.OrderBy(t => t.NomeTipo).ToList();
        }

        public TipoUsuario BuscarPorId(Guid id)
        {
            return _context.TipoUsuario.Find(id);
        }

        public TipoUsuario BuscarPorNome(string nome)
        {
            return _context.TipoUsuario.FirstOrDefault(t => t.NomeTipo == nome);
        }

        public void Adicionar(TipoUsuario tipo)
        {
            _context.TipoUsuario.Add(tipo);
            _context.SaveChanges();
        }

        public void Atualizar(TipoUsuario tipo)
        {
            if(tipo == null)
            {
                return;
            }

            TipoUsuario tipoBanco = _context.TipoUsuario.Find(tipo.TipoUsuarioID);

            if(tipoBanco == null)
            {
                return;
            }

            tipoBanco.NomeTipo = tipo.NomeTipo;

            _context.SaveChanges();
        }
    }
}
