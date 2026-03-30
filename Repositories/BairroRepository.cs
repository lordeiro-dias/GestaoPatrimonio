using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class BairroRepository : IBairroRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public BairroRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Bairro> Listar()
        {
            return _context.Bairro.OrderBy(b => b.NomeBairro).ToList();
        }

        public Bairro ObterPorId(Guid id)
        {
            return _context.Bairro.Find(id);
        }

        public Bairro BuscarPorNome(string nomeBairro, Guid cidadeId)
        {
            return _context.Bairro.FirstOrDefault(b => b.NomeBairro.ToLower() == nomeBairro.ToLower() && b.CidadeID == cidadeId);
        }

        public bool CidadeExiste(Guid cidadeId)
        {
            return _context.Cidade.Any(c => c.CidadeID == cidadeId);
        }

        public void Adicionar(Bairro bairro)
        {
            _context.Bairro.Add(bairro);
            _context.SaveChanges();
        }

        public void Atualizar(Bairro bairro)
        {
            if(bairro == null)
            {
                return;
            }

            Bairro bairroBanco = _context.Bairro.Find(bairro.BairroID);

            if(bairroBanco == null)
            {
                return;
            }

            bairroBanco.NomeBairro = bairro.NomeBairro;
            bairroBanco.CidadeID = bairro.CidadeID;

            _context.SaveChanges();
        }
    }
}
