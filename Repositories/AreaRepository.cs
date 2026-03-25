using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public AreaRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Area> Listar()
        {
            return _context.Area.OrderBy(area => area.NomeArea).ToList();
        }

        public Area BuscarPorId(Guid areaId)
        {
            return _context.Area.Find(areaId);
        }

        public Area BuscarPorNome(string nomeArea)
        {
            return _context.Area.FirstOrDefault(a => a.NomeArea.ToLower() == nomeArea.ToLower());
        }

        public void Adicionar(Area area)
        {
            _context.Area.Add(area);
            _context.SaveChanges();
        }

        public void Atualizar(Area area)
        {
            if(area == null)
            {
                return;
            }

            Area areaBanco = _context.Area.Find(area.AreaID);

            areaBanco.NomeArea = area.NomeArea;
            _context.SaveChanges();

        }
    }
}
