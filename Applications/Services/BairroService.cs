using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.BairroDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class BairroService
    {
        private readonly IBairroRepository _repository;

        public BairroService(IBairroRepository repository)
        {
            _repository = repository;
        }

        public List<ListarBairroDto> Listar()
        {
            List<Bairro> bairros = _repository.Listar();

            List<ListarBairroDto> dto = bairros.Select(b => new ListarBairroDto
            {
                BairroId = b.BairroID,
                NomeBairro = b.NomeBairro,
                CidadeId = b.CidadeID
            }).ToList();

            return dto;
        }

        public ListarBairroDto ObterPorId(Guid id)
        {
            Bairro bairro = _repository.ObterPorId(id);

            if(bairro == null)
            {
                throw new DomainException("Bairro não encontrado.");
            }

            return new ListarBairroDto
            {
                BairroId = bairro.BairroID,
                NomeBairro = bairro.NomeBairro,
                CidadeId = bairro.CidadeID
            };
        }

        public void Adicionar(CriarBairroDto bairroDto)
        {
            Validar.ValidarNome(bairroDto.NomeBairro);

            Bairro bairroExistente = _repository.BuscarPorNome(bairroDto.NomeBairro, bairroDto.CidadeId);

            if(bairroExistente != null)
            {
                throw new DomainException("Já existe um bairro cadastrado com este nome nessa cidade.");
            }

            if(!_repository.CidadeExiste(bairroDto.CidadeId))
            {
                throw new DomainException("Cidade informada não existe.");
            }

            Bairro bairro = new Bairro
            {
                NomeBairro = bairroDto.NomeBairro,
                CidadeID = bairroDto.CidadeId
            };

            _repository.Adicionar(bairro);
        }

        public void Atualizar(Guid BairroId, CriarBairroDto bairroDto)
        {
            Validar.ValidarNome(bairroDto.NomeBairro);

            Bairro bairroExistente = _repository.BuscarPorNome(bairroDto.NomeBairro, bairroDto.CidadeId);
            Bairro bairroBanco = _repository.ObterPorId(BairroId);

            if(bairroBanco == null)
            {
                throw new DomainException("Bairro não encontrado.");
            }

            if(bairroExistente != null)
            {
                throw new DomainException("Já existe um bairro cadastrado com este nome nessa cidade.");
            }

            if(!_repository.CidadeExiste(bairroDto.CidadeId))
            {
                throw new DomainException("Cidade informada não existe.");
            }

            bairroBanco.NomeBairro = bairroDto.NomeBairro;
            bairroBanco.CidadeID = bairroDto.CidadeId;

            _repository.Atualizar(bairroBanco);
        }
    }
}
