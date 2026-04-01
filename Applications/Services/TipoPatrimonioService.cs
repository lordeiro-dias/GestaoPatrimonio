using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.TipoPatrimonioDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class TipoPatrimonioService
    {
        private readonly ITipoPatrimonioRepository _repository;

        public TipoPatrimonioService(ITipoPatrimonioRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoPatrimonioDto> Listar()
        {
            List<TipoPatrimonio> tipos = _repository.Listar();

            List<ListarTipoPatrimonioDto> tiposDto = tipos.Select(t => new ListarTipoPatrimonioDto
            {
                TipoPatrimonioId = t.TipoPatrimonioID,
                NomeTipo = t.NomeTipo
            }).ToList();

            return tiposDto;
        }

        public ListarTipoPatrimonioDto BuscarPorId(Guid id)
        {
            TipoPatrimonio tipo = _repository.BuscarPorId(id);

            if (tipo == null)
            {
                throw new DomainException("Tipo de Patrimônio não encontrado.");
            }

            return new ListarTipoPatrimonioDto
            {
                TipoPatrimonioId = tipo.TipoPatrimonioID,
                NomeTipo = tipo.NomeTipo
            };
        }

        public void Adicionar(CriarTipoPatrimonioDto tipoDto)
        {
            Validar.ValidarNome(tipoDto.NomeTipo);

            TipoPatrimonio tipoExistente = _repository.BuscarPorNome(tipoDto.NomeTipo);

            if (tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de patrimônio com este nome.");
            }

            TipoPatrimonio tipo = new TipoPatrimonio
            {
                NomeTipo = tipoDto.NomeTipo
            };

            _repository.Adicionar(tipo);
        }

        public void Atualizar(Guid id, CriarTipoPatrimonioDto tipoDto)
        {
            Validar.ValidarNome(tipoDto.NomeTipo);

            TipoPatrimonio tipoExistente = _repository.BuscarPorNome(tipoDto.NomeTipo);

            TipoPatrimonio tipoBanco = _repository.BuscarPorId(id);

            if (tipoBanco == null)
            {
                throw new DomainException("Tipo de Patrimônio não encontrado.");
            }

            if (tipoExistente != null)
            {
                throw new DomainException("Já existe um tipo de patrimônio com este nome.");
            }

            tipoBanco.NomeTipo = tipoDto.NomeTipo;

            _repository.Atualizar(tipoBanco);
        }
    }
}
