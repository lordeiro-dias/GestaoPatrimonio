using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.LogPatrimonioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class LogPatrimonioService
    {
        private readonly ILogPatrimonioRepository _repository;

        public LogPatrimonioService(ILogPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarLogPatrimonioDto> Listar()
        {
            List<LogPatrimonio> logs = _repository.Listar();
            List<ListarLogPatrimonioDto> logsDto = logs.Select(l => new ListarLogPatrimonioDto
            {
                LogPatrimonioId = l.LogPatrimonioID,
                DataTransferencia = l.DataTransferencia,
                PatrimonioId = l.PatrimonioID,
                DemonicacaoPatrimonio = l.Patrimonio.Denominacao,
                TipoAlteracao = l.TipoAlteracao.Tipo,
                StatusPatrimonio = l.StatusPatrimonio.Status,
                Usuario = l.Usuario.Nome,
                Localizacao = l.Local.Nome
            }).ToList();

            return logsDto;
        }

        public List<ListarLogPatrimonioDto> BuscarPorPatrimonio(Guid id)
        {
            List<LogPatrimonio> logs = _repository.BuscarPorPatrimonio(id);

            if(logs == null)
            {
                throw new DomainException("Patrimonio não encontrado.");
            }

            List<ListarLogPatrimonioDto> logsDto = logs.Select(l => new ListarLogPatrimonioDto
            {
                LogPatrimonioId = l.LogPatrimonioID,
                DataTransferencia = l.DataTransferencia,
                PatrimonioId = l.PatrimonioID,
                DemonicacaoPatrimonio = l.Patrimonio.Denominacao,
                TipoAlteracao = l.TipoAlteracao.Tipo,
                StatusPatrimonio = l.StatusPatrimonio.Status,
                Usuario = l.Usuario.Nome,
                Localizacao = l.Local.Nome
            }).ToList();

            return logsDto;
        }
    }
}
