using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class LocalRepository : ILocalRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public LocalRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Local> Listar()
        {
            return _context.Local.OrderBy(l => l.Nome).ToList();
        }

        public Local BuscarPorId(Guid localID)
        {
            return _context.Local.Find(localID);
        }

        public Local BuscarPorNome(string nomeLocal, Guid areaId)
        {
            return _context.Local.FirstOrDefault(l => l.Nome.ToLower() == nomeLocal.ToLower() && l.AreaID == areaId);
        }

        public void Adicionar(Local local)
        {
            _context.Local.Add(local);
            _context.SaveChanges();
        }

        public bool AreaExiste(Guid areaID)
        {
            return _context.Area.Any(a => a.AreaID == areaID);
        }

        public void Atualizar(Local local)
        {
            if(local == null)
            {
                return;
            }

            Local localBanco = _context.Local.Find(local.LocalID);

            if(localBanco == null)
            {
                return;
            }

            localBanco.Nome = local.Nome;
            localBanco.LocalSAP = local.LocalSAP;
            localBanco.DescricaoSAP = local.DescricaoSAP;
            localBanco.AreaID = local.AreaID;

            _context.SaveChanges();
        }
    }
}
