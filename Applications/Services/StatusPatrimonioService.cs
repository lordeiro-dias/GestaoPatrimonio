using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.StatusPatrimonioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class StatusPatrimonioService
    {
        private readonly IStatusPatrimonioRepository _repository;

        public StatusPatrimonioService(IStatusPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarStatusPatrimonioDto> Listar()
        {
            List<StatusPatrimonio> statusP = _repository.Listar();
            List<ListarStatusPatrimonioDto> statusDto = statusP.Select(s => new ListarStatusPatrimonioDto
            {
                statusPatrimonioId = s.StatusPatrimonioID,
                nomeStatus = s.Status
            }).ToList();

            return statusDto;
        }

        public ListarStatusPatrimonioDto BuscarPorId(Guid id)
        {
            StatusPatrimonio status = _repository.BuscarPorId(id);

            return new ListarStatusPatrimonioDto
            {
                statusPatrimonioId = status.StatusPatrimonioID,
                nomeStatus = status.Status
            };
        }

        public void Adicionar(CriarStatusPatrimonioDto dto)
        {
            Validar.ValidarNome(dto.nomeStatus);
            StatusPatrimonio statusExistente = _repository.BuscarPorNome(dto.nomeStatus);

            if(statusExistente != null)
            {
                throw new DomainException("Já existe um Status de Patrimônio com esse nome.");
            }

            StatusPatrimonio status = new StatusPatrimonio
            {
                Status = dto.nomeStatus
            };

            _repository.Adicionar(status);
        }

        public void Atualizar(Guid id, CriarStatusPatrimonioDto dto)
        {
            Validar.ValidarNome(dto.nomeStatus);
            StatusPatrimonio statusExistente = _repository.BuscarPorNome(dto.nomeStatus);

            if (statusExistente != null)
            {
                throw new DomainException("Já existe um Status de Patrimônio com esse nome.");
            }

            StatusPatrimonio statusBanco = _repository.BuscarPorId(id);

            if (statusBanco == null)
            {
                throw new DomainException("Status de Patrimônio não encontado.");
            }

            statusBanco.Status = dto.nomeStatus;

            _repository.Atualizar(statusBanco);
        }
    }
}
