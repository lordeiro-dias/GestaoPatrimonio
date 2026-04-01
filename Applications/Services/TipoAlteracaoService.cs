using GerenciamentoPatrimonio.Applications.Regras;
using GerenciamentoPatrimonio.Domains;
using GerenciamentoPatrimonio.DTOs.TipoAlteracaoDto;
using GerenciamentoPatrimonio.Exceptions;
using GerenciamentoPatrimonio.Interfaces;

namespace GerenciamentoPatrimonio.Applications.Services
{
    public class TipoAlteracaoService
    {
        private readonly ITipoAlteracaoRepository _repository;

        public TipoAlteracaoService(ITipoAlteracaoRepository repository)
        {
            _repository = repository;
        }

        public List<ListarTipoAlteracaoDto> Listar()
        {
            List<TipoAlteracao> tipos = _repository.Listar();

            List<ListarTipoAlteracaoDto> tipoDto = tipos.Select(t => new ListarTipoAlteracaoDto
            {
                TipoAlteracaoId = t.TipoAlteracaoID,
                NomeTipo = t.Tipo
            }).ToList();

            return tipoDto;
        }

        public ListarTipoAlteracaoDto BuscarPorId(Guid id)
        {
            TipoAlteracao tipo = _repository.BuscarPorId(id);

            if(tipo == null)
            {
                throw new DomainException("Tipo de Alteração não encontrada.");
            }

            return new ListarTipoAlteracaoDto
            {
                TipoAlteracaoId = tipo.TipoAlteracaoID,
                NomeTipo = tipo.Tipo
            };
        }

        public void Adicionar(CriarTipoAlteracaoDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoAlteracao tipoExiste = _repository.BuscarPorNome(dto.NomeTipo);

            if(tipoExiste != null)
            {
                throw new DomainException("Tipo de Alteração já existe.");
            }

            TipoAlteracao tipo = new TipoAlteracao
            {
                Tipo = dto.NomeTipo
            };

            _repository.Adicionar(tipo);
        }

        public void Atualizar(Guid id, CriarTipoAlteracaoDto dto)
        {
            Validar.ValidarNome(dto.NomeTipo);

            TipoAlteracao tipoExiste = _repository.BuscarPorNome(dto.NomeTipo);

            TipoAlteracao tipoBanco = _repository.BuscarPorId(id);

            if (tipoExiste != null)
            {
                throw new DomainException("Tipo de Alteração já existe.");
            }

            if(tipoBanco == null)
            {
                throw new DomainException("Tipo de Alteração não encontrada.");
            }

            tipoBanco.Tipo = dto.NomeTipo;

            _repository.Atualizar(tipoBanco);
        }
    }
}
