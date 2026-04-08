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

        public Patrimonio BuscarPorNumeroPatrimonio(string numeroPatrimonio, Guid? patrimonioId = null)
        {
            var consulta = _context.Patrimonio.AsQueryable();

            if(patrimonioId.HasValue)
            {
                consulta = consulta.Where(p => p.PatrimonioID != patrimonioId);
            }

            return consulta.FirstOrDefault(p => p.NumeroPatrimonio == numeroPatrimonio);
        }

        public bool LocalizacaoExiste(Guid localId)
        {
            return _context.Local.Any(l => l.LocalID == localId);
        }

        public bool StatusPatrimonioExiste(Guid statusPatrimonioId)
        {
            return _context.StatusPatrimonio.Any(s => s.StatusPatrimonioID == statusPatrimonioId);
        }

        public void Adicionar(Patrimonio patrimonio)
        {
            _context.Patrimonio.Add(patrimonio);
            _context.SaveChanges();
        }

        public void Atualizar(Patrimonio patrimonio)
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

            patrimonioBanco.Denominacao = patrimonio.Denominacao;
            patrimonioBanco.NumeroPatrimonio = patrimonio.NumeroPatrimonio;
            patrimonioBanco.Valor = patrimonio.Valor;
            patrimonioBanco.Imagem = patrimonio.Imagem;
            patrimonioBanco.LocalID = patrimonio.LocalID;
            patrimonioBanco.TipoPatrimonioID = patrimonio.TipoPatrimonioID;

            _context.SaveChanges();
        }

        public void AtualizarStatus(Patrimonio patrimonio)
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

            patrimonioBanco.StatusPatrimonioID = patrimonio.StatusPatrimonioID;

            _context.SaveChanges();
        }
    }
}
