using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoPatrimonio.Repositories
{
    public class CidadeRepository : ICidadeRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public CidadeRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Cidade> Listar()
        {
            return _context.Cidade.OrderBy(cidade => cidade.NomeCidade).ToList();
        }

        public Cidade BuscarPorId(Guid cidadeId)
        {
            return _context.Cidade.Find(cidadeId);
        }

        public Cidade BuscarPorNomeEstado(string nomeCidade, string nomeEstado)
        {
            return _context.Cidade.FirstOrDefault(c => c.NomeCidade.ToLower() == nomeCidade.ToLower() && c.Estado.ToLower() == nomeEstado.ToLower());
        }

        public void Adicionar(Cidade cidade)
        {
            _context.Cidade.Add(cidade);
            _context.SaveChanges();
        }

        public void Atualizar(Cidade cidade)
        {
            if(cidade == null)
            {
                return;
            }

            Cidade cidadeBanco = _context.Cidade.Include(c => c.Bairro).FirstOrDefault(c => c.CidadeID == cidade.CidadeID);

            if(cidadeBanco == null)
            {
                return;
            }

            cidadeBanco.NomeCidade = cidade.NomeCidade;
            cidadeBanco.Estado = cidade.Estado;
            

        }
    }
}
