using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.StatusTransferenciaDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class StatusTransferenciaService
    {
        private readonly IStatusTransferenciaRepository _repository;

        public StatusTransferenciaService(IStatusTransferenciaRepository repository)
        {
            _repository = repository;
        }

        public List<ListarStatusTransferenciaDto> Listar()
        {
            List<StatusTransferencia> statusT = _repository.Listar();
            List<ListarStatusTransferenciaDto> statusDto = statusT.Select(s => new ListarStatusTransferenciaDto
            {
                StatusTransferenciaId = s.StatusTransferenciaID,
                NomeStatus = s.Status
            }).ToList();

            return statusDto;
        }

        public ListarStatusTransferenciaDto BuscarPorId(Guid id)
        {
            StatusTransferencia status = _repository.BuscarPorId(id);

            return new ListarStatusTransferenciaDto
            {
                StatusTransferenciaId = status.StatusTransferenciaID,
                NomeStatus = status.Status
            };
        }

        public void Adicionar(CriarStatusTransferenciaDto dto)
        {
            Validar.ValidarNome(dto.NomeStatus);
            StatusTransferencia statusExistente = _repository.BuscarPorNome(dto.NomeStatus);

            if(statusExistente != null)
            {
                throw new DomainException("Já existe um Status de Transferência com esse nome.");
            }

            StatusTransferencia statusT = new StatusTransferencia
            {
                Status = dto.NomeStatus
            };

            _repository.Adicionar(statusT);
        }

        public void Atualizar(Guid id, CriarStatusTransferenciaDto dto)
        {
            Validar.ValidarNome(dto.NomeStatus);
            StatusTransferencia statusExistente = _repository.BuscarPorNome(dto.NomeStatus);

            if (statusExistente != null)
            {
                throw new DomainException("Já existe um Status de Transferência com esse nome.");
            }

            StatusTransferencia statusBanco = _repository.BuscarPorId(id);

            if(statusBanco == null)
            {
                throw new DomainException("Status de Transferência não encontrado.");
            }
            
            statusBanco.Status = dto.NomeStatus;

            _repository.Adicionar(statusBanco);
        }
    }
}
