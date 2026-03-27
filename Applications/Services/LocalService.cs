using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.LocalDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class LocalService
    {
        private readonly ILocalRepository _repository;

        public LocalService(ILocalRepository repository)
        {
            _repository = repository;
        }

        public List<ListarLocalDto> Listar()
        {
            List<Local> localizacoes = _repository.Listar();

            List<ListarLocalDto> localDto = localizacoes.Select(l => new ListarLocalDto
            {
                LocalID = l.LocalID,
                NomeLocal = l.Nome,
                LocalSAP = l.LocalSAP,
                DescricaoSAP = l.DescricaoSAP,
                AreaID = l.AreaID
            }).ToList();

            return localDto;
        }

        public ListarLocalDto BuscarPorId(Guid localID)
        {
            Local local = _repository.BuscarPorId(localID);

            if(local == null)
            {
                throw new DomainException("Localização não encontrada.");
            }

            return new ListarLocalDto
            {
                LocalID = local.LocalID,
                NomeLocal = local.Nome,
                LocalSAP = local.LocalSAP,
                DescricaoSAP = local.DescricaoSAP,
                AreaID = local.AreaID
            };
        }

        public void Adicionar(CriarLocalDto localDto)
        {
            Validar.ValidarNome(localDto.NomeLocal);

            Local localExistente = _repository.BuscarPorNome(localDto.NomeLocal, localDto.AreaID);

            if(localExistente != null)
            {
                throw new DomainException("Já existe um Local cadastrado com este nome nessa área.");
            }

            if(!_repository.AreaExiste(localDto.AreaID))
            {
                throw new DomainException("Área informada não existe.");
            }

            Local local = new Local
            {
                Nome = localDto.NomeLocal,
                LocalSAP = localDto.LocalSAP,
                DescricaoSAP = localDto.DescricaoSAP,
                AreaID = localDto.AreaID
            };

            _repository.Adicionar(local);
        }

        public void Atualizar(Guid localId, CriarLocalDto localDto)
        {
            Validar.ValidarNome(localDto.NomeLocal);

            Local localExistente = _repository.BuscarPorNome(localDto.NomeLocal, localDto.AreaID);

            Local localBanco = _repository.BuscarPorId(localId);

            if(localBanco == null)
            {
                throw new DomainException("Localização não encontrada.");
            }

            if (localExistente != null)
            {
                throw new DomainException("Já existe um Local cadastrado com este nome nessa área.");
            }

            if(!_repository.AreaExiste(localDto.AreaID))
            {
                throw new DomainException("Área informada não existe");
            }

            localBanco.Nome = localDto.NomeLocal;
            localBanco.LocalSAP = localDto.LocalSAP;
            localBanco.DescricaoSAP = localDto.DescricaoSAP;
            localBanco.AreaID = localDto.AreaID;

            _repository.Atualizar(localBanco);
        }
    }
}
