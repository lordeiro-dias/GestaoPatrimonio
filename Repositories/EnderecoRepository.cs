using GerenciamentoPatrimonio.Contexts;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly GerenciamentoPatrimoniosContext _context;

        public EnderecoRepository(GerenciamentoPatrimoniosContext context)
        {
            _context = context;
        }

        public List<Endereco> Listar()
        {
            return _context.Endereco.OrderBy(e => e.Logradouro).ToList();
        }

        public Endereco BuscarPorId(Guid tipoUsuarioId)
        {
            return _context.Endereco.Find(tipoUsuarioId);
        }

        public Endereco BuscarPorLogradouroENumero(string logradouro, int? numero, Guid bairroId, Guid? enderecoId = null)
        {
            var consulta = _context.Endereco.AsQueryable();

            if (enderecoId.HasValue)
            {
                consulta = consulta.Where(endereco => endereco.EnderecoID != enderecoId.Value);
            }

            return consulta.FirstOrDefault(e => e.Logradouro.ToLower() == logradouro.ToLower() && e.Numero == numero && e.BairroID == bairroId);
        }

        public bool BairroExiste(Guid bairroId)
        {
            return _context.Bairro.Any(b => b.BairroID == bairroId);
        }

        public void Adicionar(Endereco endereco)
        {
            _context.Endereco.Add(endereco);
            _context.SaveChanges();
        }

        public void Atualizar(Endereco endereco)
        {
            if(endereco == null)
            {
                return;
            }

            Endereco enderecoBanco = _context.Endereco.Find(endereco.EnderecoID);

            if(enderecoBanco == null)
            {
                return; 
            }

            enderecoBanco.Logradouro = endereco.Logradouro;
            enderecoBanco.Numero = endereco.Numero;
            enderecoBanco.CEP = endereco.CEP;
            enderecoBanco.Complemento = endereco.Complemento;
            enderecoBanco.BairroID = endereco.BairroID;

            _context.SaveChanges();
        }
    }
}
