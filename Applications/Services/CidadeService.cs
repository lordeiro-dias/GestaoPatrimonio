using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.CidadeDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class CidadeService
    {
        private readonly ICidadeRepository _repository;

        public CidadeService(ICidadeRepository repository)
        {
            _repository = repository;
        }

        public List<ListarCidadeDto> Listar()
        {
            List<Cidade> cidades = _repository.Listar();
            List<ListarCidadeDto> cidadesDto = cidades.Select(cidades => new ListarCidadeDto
            {
                CidadeID = cidades.CidadeID,
                NomeCidade = cidades.NomeCidade,
                Estado = cidades.Estado
            }).ToList();

            return cidadesDto;
        }

        public ListarCidadeDto BuscarPorId(Guid cidadeId)
        {
            Cidade cidadeBanco = _repository.BuscarPorId(cidadeId);
            
            if(cidadeBanco == null)
            {
                throw new DomainException("Cidade não encontrada");
            }

            ListarCidadeDto cidadeDto = new ListarCidadeDto
            {
                CidadeID = cidadeBanco.CidadeID,
                NomeCidade = cidadeBanco.NomeCidade,
                Estado = cidadeBanco.Estado
            };

            return cidadeDto;
        }

        public void Adicionar(CriarCidadeDto cidade)
        {
            Validar.ValidarNome(cidade.NomeCidade);

            Cidade cidadeExistente = _repository.BuscarPorNomeEstado(cidade.NomeCidade, cidade.Estado);

            if(cidadeExistente != null)
            {
                throw new DomainException("Já existe uma Cidade cadastrada com esse nome neste Estado.");
            }

            Cidade cidadeCriada = new Cidade
            {
                NomeCidade = cidade.NomeCidade,
                Estado = cidade.Estado
            };

            _repository.Adicionar(cidadeCriada);
        }

        public void Atualizar(Guid cidadeId, CriarCidadeDto cidadeDto)
        {
            Validar.ValidarNome(cidadeDto.NomeCidade);

            Cidade cidadeBanco = _repository.BuscarPorId(cidadeId);

            if(cidadeBanco == null)
            {
                throw new DomainException("Cidade não encontrada.");
            }

            Cidade cidadeExistente = _repository.BuscarPorNomeEstado(cidadeDto.NomeCidade, cidadeDto.Estado);

            if(cidadeExistente != null)
            {
                throw new DomainException("Já existe uma Cidade cadastrada com esse nome neste Estado.");
            }

            cidadeBanco.NomeCidade = cidadeDto.NomeCidade;
            cidadeBanco.Estado = cidadeDto.Estado;

            _repository.Atualizar(cidadeBanco);
        }
    }
}
